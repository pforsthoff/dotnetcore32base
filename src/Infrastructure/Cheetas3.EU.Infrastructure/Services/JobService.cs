
using AutoMapper;
using Cheetas3.EU.Application.Common.Events;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Infrastructure.Services
{
    public class JobService : IJobService
    {
        private readonly IAppConfigService _configurationService;
        private readonly IApplicationDbContext _context;
        private readonly IDockerService _dockerService;
        private readonly IKubernetesService _kubernetesService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IDateTime _dateTime;
        private readonly ILogger<JobService> _logger;
        private readonly IMapper _mapper;
        private bool _dockerJobRunning;
        private bool _k8sJobRunning;
        private readonly Queue<Slice> _dockerQueue = new Queue<Slice>();
        private readonly Queue<Slice> _k8sQueue = new Queue<Slice>();

        public JobService(IApplicationDbContext context, IDockerService dockerService,
                          IKubernetesService kubernetesService, IDateTime dateTime,
                          IMessageQueueService messageQueueService, ILogger<JobService> logger,
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
            _messageQueueService.MessageReceivedEventHandler += messageQueueService_MessageReceivedEventHandlerAsync;
        }

        public async Task<JobStatus> ProcessJob(Job job, TargetPlatform platform)
        {


            switch (platform)
            {
                case TargetPlatform.HostOS:
                    _messageQueueService.ConsumeMessage();
                    break;
                case TargetPlatform.Docker:
                    await ExecuteJobWithDockerAsync(job);
                    break;
                case TargetPlatform.Kubernetes:
                    await ExecuteJobWithKubernetesAysnc(job);
                    break;
                default:
                    break;
            }

            return JobStatus.Completed;
        }

        private async Task messageQueueService_MessageReceivedEventHandlerAsync(object sender, MessageEntityEventArgs<Slice> e)
        {
            var slice = e.Entity;
            var sliceCompleted = slice.Status == SliceStatus.Completed;

            _logger.LogInformation($"Event from Message Service with SliceID:{slice.Id}, {slice.Status} was received.");

            if (sliceCompleted) slice.SliceCompleted = _dateTime.Now;

            await UpdateDatabaseAsync(slice);

            if (sliceCompleted) await ProcessQueues(slice);
        }

        private async Task ExecuteJobSliceWithDockerAsync()
        {
            var queuedSlice = _k8sQueue.Dequeue();
            var slice = _context.Slices.Single(r => r.Id == queuedSlice.Id);
            slice.TargetPlatform = TargetPlatform.Docker;
            slice.Status = SliceStatus.Starting;
            slice.SliceStarted = _dateTime.Now;

            await UpdateDatabaseAsync(slice);

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
            var queuedSlice = _k8sQueue.Dequeue();
            var slice = _context.Slices.Single(r => r.Id == queuedSlice.Id);
            slice.TargetPlatform = TargetPlatform.Kubernetes;
            slice.Status = SliceStatus.Starting;
            slice.SliceStarted = _dateTime.Now;

            await UpdateDatabaseAsync(slice);

            var client = _kubernetesService.GetKubernetesClient();
            var job = _kubernetesService.GetEUConverterJob(slice.Id);
            await client.CreateNamespacedJobWithHttpMessagesAsync(job, "default");
        }

        private async Task ExecuteJobWithDockerAsync(Job job)
        {
            //Add Slices To Docker Queue
            foreach (var slice in job.Slices.Where(p => p.Status == SliceStatus.Pending))
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

        private async Task ExecuteJobWithKubernetesAysnc(Job job)
        {
            //Add Slices To k8s Queue
            foreach (var slice in job.Slices.Where(p => p.Status == SliceStatus.Pending))
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

        private async Task ProcessQueues(Slice slice)
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
                        //Update the Job as completed.
                        //Will need to track what job is completed as new jobs can be started while someother job is already running.
                        break;
                    case TargetPlatform.Kubernetes:
                        if (_k8sQueue.Any())
                            await ExecuteJobSliceWithKubernetesAsync();
                        else
                            _k8sJobRunning = false;
                        //Update the Job as completed.
                        //Will need to track what job is completed as new jobs can be started while someother job is already running.
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task SaveEntityUpdatesAsync()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task UpdateDatabaseAsync(Slice slice)
        {
            var entity = await _context.Slices.FindAsync(slice.Id);
            entity.Status = slice.Status;
            _logger.LogInformation($"Updating Slices Table SliceId:{entity.Id}, Status:{entity.Status}");
            //_mapper.Map(slice, entity);
            await SaveEntityUpdatesAsync();
        }
    }
}
