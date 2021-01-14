using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.JobProvisioningTasks.Commands.UpdateTask
{
    public class UpdateJobProvisioningTaskCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime? JobProvisionedDateTime { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateJobProvisioningTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJobProvisioningTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.JobProvisioningTasks.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(JobProvisioningTask), request.Id);
            }

            entity.Status = request.Status;
            entity.JobProvisionedDateTime = request.JobProvisionedDateTime;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
