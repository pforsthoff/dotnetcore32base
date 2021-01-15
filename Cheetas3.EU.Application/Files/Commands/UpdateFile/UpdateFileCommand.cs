using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Files.Commands.UpdateFile
{
    public class UpdateFileCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime? JobProvisionedDateTime { get; set; }
        public Domain.Enums.FileStatus Status { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Files.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(File), request.Id);
            }

            entity.Status = request.Status;
            entity.JobProvisionedDateTime = request.JobProvisionedDateTime;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
