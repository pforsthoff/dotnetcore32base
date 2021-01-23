using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cheetas3.EU.Application.Common.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using k8s.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Cheetas3.EU.Application.Features.Jobs.Comands.ExecuteJob
{
    public class ExecuteJobCommand : IRequest<int>
    {
        public int Id { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
    }

    public class ExecuteJobCommandHandler : IRequestHandler<ExecuteJobCommand, int>
    {
        private readonly IConfigurationProvisioningService _configurationService;
        private readonly IApplicationDbContext _context;
        private readonly IDockerService _dockerService;
        private readonly IKubernetesService _kubernetesService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IDateTime _dateTime;
        private readonly ILogger<ExecuteJobCommandHandler> _logger;
        private readonly IMapper _mapper;
        private Queue<Slice> _dockerQueue = new Queue<Slice>();
        private Queue<Slice> _k8sQueue = new Queue<Slice>();
        private bool _dockerJobRunning;
        private bool _k8sJobRunning;

        public ExecuteJobCommandHandler(IApplicationDbContext context, IDockerService dockerService,
                                        IKubernetesService kubernetesService, IDateTime dateTime,
                                        IMessageQueueService messageQueueService, ILogger<ExecuteJobCommandHandler> logger,
                                        IConfigurationProvisioningService configurationService, IMapper mapper)
        {
            _context = context;
            _dockerService = dockerService;
            _kubernetesService = kubernetesService;
            _messageQueueService = messageQueueService;
            _dateTime = dateTime;
            _mapper = mapper;
            _logger = logger;
            _configurationService = configurationService;
            _messageQueueService.MessageReceivedEventHandler += MessageQueueService_MessageReceivedEventHandler;
        }

        private void MessageQueueService_MessageReceivedEventHandler(object sender, Common.Events.MessageEntityEventArgs<Slice> e)
        {
            var slice = e.Entity;
            _logger.LogInformation($"Event from Message Service with SliceID:{slice.Id} was received.");

            ProcessQueues(slice);

        }

        public async Task ProcessQueues(Slice slice)
        {
            if (slice.Status == SliceStatus.Completed)
            {
                //Check The  QueueType To See if More Jobs Waiting
                switch (slice.TargetPlatform)
                {
                    case TargetPlatform.Docker:
                        if (_dockerQueue.Any())
                            await ExecuteJobSliceWithDockerAsync();
                        else
                            _dockerJobRunning = false;
                        break;
                    case TargetPlatform.Kubernetes:
                        if (_k8sQueue.Any())
                        {
                        }
                        break;
                    default:
                        break;
                }

            }
        }


        public async Task UpdateDataBaseAsync(Slice slice)
        {
            var entity = await _context.Slices.FindAsync(slice.Id);
            _mapper.Map(slice, entity);
            //await _context.SaveChangesAsync(CancellationToken.None);
        }


        public async Task<int> Handle(ExecuteJobCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Jobs.Include(j => j.Slices)
                 .Where(p => p.Id == request.Id && p.Status != JobStatus.Completed)
                 .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }

            entity.Status = JobStatus.InProgress;
            entity.StartedDateTime = _dateTime.Now;
            //await _context.SaveChangesAsync(cancellationToken);

            var platform = request.TargetPlatform;
            switch (platform)
            {
                case TargetPlatform.HostOS:
                    _messageQueueService.ConsumeMessage();
                    break;
                case TargetPlatform.Docker:
                    _ = ExecuteJobWithDockerAsync(entity);
                    break;
                case TargetPlatform.Kubernetes:
                    var client = _kubernetesService.GetKubernetesClient();
                    V1Job job;
                    foreach (var slice in entity.Slices.Where( r => r.Status == SliceStatus.Pending))
                    {
                        job = _kubernetesService.GetEUConverterJob(slice.Id);
                        await client.CreateNamespacedJobWithHttpMessagesAsync(job, "default");
                    }
                    break;
                default:
                    break;
            }

            return 1;
        }

        private async Task ExecuteJobSliceWithDockerAsync()
        {
            var slice = _dockerQueue.Dequeue();
            var envVariables = new List<string>
            {
                $"RetryCount={_configurationService.RetryCount}",
                $"Sliceid={slice.Id}",
                $"SleepDuration=300000",
                $"ServiceHealthEndPoint=http://localhost:5000/actuator/health"
            };
            await _dockerService.CreateAndStartContainerAsync(_configurationService.Image, envVariables);
        }

        private async Task ExecuteJobWithDockerAsync(Job entity)
        {
            //_dockerService.CreateDockerClient("http://192.168.1.20:2375");

            //Add Slices To Queue
            foreach (var slice in entity.Slices.Where(p => p.Status == SliceStatus.Pending))
                _dockerQueue.Enqueue(slice);

            if (!_dockerJobRunning)
            {
                _dockerJobRunning = true;
                _dockerService.CreateDockerClient();
                var image = _configurationService.Image;
                await _dockerService.PullImageAsync(image);
                var envVariables = new List<string>
                {
                    $"RetryCount={_configurationService.RetryCount}",
                    $"SleepDuration={_configurationService.DevAttributeContainerLifeDuration}",
                    $"ServiceHealthEndPoint={_configurationService.}"
                };

                for (int i = 0; i < _configurationService.MaxConcurrency; i++)
                {
                    if (_dockerQueue.Any())
                    {
                        var slice = _dockerQueue.Dequeue();
                        envVariables.Add($"SliceId={slice.Id}");
                        await _dockerService.CreateAndStartContainerAsync(image, envVariables);
                        slice.Status = SliceStatus.Starting;
                        slice.TargetPlatform = TargetPlatform.Docker;
                        await UpdateDataBaseAsync(slice);
                        envVariables.RemoveAt(3);
                    }
                }
            }
        }
    }
}
