using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cheetas3.EU.Domain.Enums;

namespace Cheetas3.EU.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileCommand : IRequest<int>
    {
        public  DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FileStatus? Status { get; set; }
    }

    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var entity = new File
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = (FileStatus)request.Status
            };
            _context.Files.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
