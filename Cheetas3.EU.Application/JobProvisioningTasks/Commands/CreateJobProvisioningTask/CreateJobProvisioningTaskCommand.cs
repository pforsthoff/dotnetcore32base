using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.JobProvisioningTasks.Commands.CreateTask
{
    public class CreateJobProvisioningTaskCommand : IRequest<int>
    {
        public Int64 FileTimeSpan { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }

    public class CreateJobProvisioningTaskCommandHandler : IRequestHandler<CreateJobProvisioningTaskCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateJobProvisioningTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateJobProvisioningTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = new JobProvisioningTask
            {
                FileTimeSpan = request.FileTimeSpan,
                Status = Domain.Enums.TaskStatus.Unprovisioned
            };
            _context.JobProvisioningTasks.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
