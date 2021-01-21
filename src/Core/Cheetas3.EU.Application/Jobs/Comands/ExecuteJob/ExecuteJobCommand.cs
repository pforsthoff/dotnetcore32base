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

namespace Cheetas3.EU.Application.Jobs.Comands.ExecuteJob
{
    public class ExecuteJobCommand : IRequest<int>
    {
        public int Id { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
    }

    public class ExecuteJobCommandHandler : IRequestHandler<ExecuteJobCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDockerService _dockerService;
        private readonly IKubernetesService _kubernetesService;
        private readonly IDateTime _dateTime;

        public ExecuteJobCommandHandler(IApplicationDbContext context, IDockerService dockerService, 
                                        IKubernetesService kubernetesService, IDateTime dateTime)
        {
            _context = context;
            _dockerService = dockerService;
            _kubernetesService = kubernetesService;
            _dateTime = dateTime;
        }
        public async Task<int> Handle(ExecuteJobCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Jobs.Include(j => j.Slices)
                 .Where(p => p.Id == request.Id)
                 .SingleOrDefaultAsync(cancellationToken);


            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }

            entity.Status = JobStatus.InProgress;
            entity.StartedDateTime = _dateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);

            var platform = request.TargetPlatform;
            switch (platform)
            {
                case TargetPlatform.HostOS:
                    break;
                case TargetPlatform.Docker:
                    //_dockerService.CreateDockerClient("http://192.168.1.20:2375");
                    _dockerService.CreateDockerClient();
                    var imageName = "pguerette/euconverter:latest";
                    await _dockerService.PullImageAsync(imageName);

                    var envVariables = new List<string>();
                    envVariables.Add($"SleepDuration=300000");
                    envVariables.Add($"ServiceHealthEndPoint=http://localhost:5000/actuator/health");
                    foreach (var slice in entity.Slices)
                    {
                        envVariables.Add($"SliceId={slice.Id}");
                        await _dockerService.CreateAndStartContainerAsync(imageName, envVariables);
                        envVariables.RemoveAt(2);
                    }
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
    }
}
