using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {

            //TODO: Prevent Deletion if File already has job attached.

            var entity = await _context.Files
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(File), request.Id);
            }

            _context.Files.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
