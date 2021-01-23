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
        private readonly IAppConfigService _configurationService;
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
                                        IAppConfigService configurationService, IMapper mapper)
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
                            await ExecuteJobSliceWithKubernetesAsync();
                        else
                            _k8sJobRunning = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task SaveEntityUpdatesAsync()
        {
           await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task UpdateDataBaseAsync(Slice slice)
        {
            var entity = await _context.Slices.FindAsync(slice.Id);
            _mapper.Map(slice, entity);
            await SaveEntityUpdatesAsync();
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
            await SaveEntityUpdatesAsync();

            var platform = request.TargetPlatform;
            switch (platform)
            {
                case TargetPlatform.HostOS:
                    _messageQueueService.ConsumeMessage();
                    break;
                case TargetPlatform.Docker:
                    await ExecuteJobWithDockerAsync(entity);
                    break;
                case TargetPlatform.Kubernetes:
                    await ExecuteJobWithKubernetesAysnc(entity);
                    break;
                default:
                    break;
            }

            return 1;
        }

        private async Task ExecuteJobSliceWithDockerAsync()
        {
            var slice = _dockerQueue.Dequeue();
            slice.TargetPlatform = TargetPlatform.Docker;
            slice.Status = SliceStatus.Starting;
            await UpdateDataBaseAsync(slice);

            var envVariables = new List<string>
            {
                $"RetryCount={_configurationService.RetryCount}",
                $"Sliceid={slice.Id}",
                $"SleepDuration={_configurationService.DevAttributeContainerLifeDuration}"
            };

            await _dockerService.CreateAndStartContainerAsync(_configurationService.Image, envVariables);
        }

        private async Task ExecuteJobSliceWithKubernetesAsync()
        {
            var slice = _k8sQueue.Dequeue();
            slice.TargetPlatform = TargetPlatform.Kubernetes;
            slice.Status = SliceStatus.Starting;
            await UpdateDataBaseAsync(slice);

            var client = _kubernetesService.GetKubernetesClient();
            var job = _kubernetesService.GetEUConverterJob(slice.Id);
            await client.CreateNamespacedJobWithHttpMessagesAsync(job, "default");
        }

        private async Task ExecuteJobWithDockerAsync(Job entity)
        {
            //Add Slices To Docker Queue
            foreach (var slice in entity.Slices.Where(p => p.Status == SliceStatus.Pending))
                _dockerQueue.Enqueue(slice);

            if (!_dockerJobRunning)
            {
                _dockerJobRunning = true;
                _dockerService.CreateDockerClient(_configurationService.DockerHostUrl);

                string image = _configurationService.Image;
                await _dockerService.PullImageAsync(image);

                for (int i = 0; i < _configurationService.MaxConcurrency; i++)
                {
                    await ExecuteJobSliceWithDockerAsync();
                }
            }
        }

        private async Task ExecuteJobWithKubernetesAysnc(Job entity)
        {
            //Add Slices To k8s Queue
            foreach (var slice in entity.Slices.Where(p => p.Status == SliceStatus.Pending))
                _k8sQueue.Enqueue(slice);

            if (!_k8sJobRunning)
            {
                _k8sJobRunning = true;

                for (int i = 0; i < _configurationService.MaxConcurrency; i++)
                {
                    await ExecuteJobSliceWithKubernetesAsync();
                }
            }
        }
    }
}
