using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Jobs.Comands.UpdateJob
{
    public class UpdateJobCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime? DateTimeJobStarted { get; set; }
        public DateTime? DateTimeJobCompleted { get; set; }
        public JobStatus Status { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Jobs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }

            entity.Status = request.Status;
            entity.DateTimeJobCompleted = request.DateTimeJobCompleted;
            entity.DateTimeJobStarted = request.DateTimeJobStarted;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
