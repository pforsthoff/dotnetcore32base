using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Domain.Events;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cheetas3.EU.Application.Jobs.Comands.ExecuteJob
{
    public class ExecuteJobCommand : IRequest<int>
    {
        public int JobId { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
    }

    public class ExecuteJobCommandHandler : IRequestHandler<ExecuteJobCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDockerService _dockerService;

        public ExecuteJobCommandHandler(IApplicationDbContext context, IDockerService dockerService)
        {
            _context = context;
            _dockerService = dockerService;
        }

        public async Task<int> Handle(ExecuteJobCommand request, CancellationToken cancellationToken)
        {
            var entity = new Job
            {
                Status = JobStatus.InProgress
            };

            var platform = request.TargetPlatform;

            _dockerService.CreateDockerClient();

            var imageName = "pguerette/euconversion:latest";
            _dockerService.PullImageAsync(imageName);

            var envVariables = new List<string>();
            envVariables.Add("SliceId=1");
            envVariables.Add("SleepDuration=300000");
            await _dockerService.CreateAndStartContainerAsync(imageName, envVariables);

            //entity.DomainEvents.Add(new JobCreatedEvent(entity));

            //_context.Jobs.Update(entity);

            //await _context.SaveChangesAsync(cancellationToken);

            //return entity.Id;
            return 1;
        }
    }
}
