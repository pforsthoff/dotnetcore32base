using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Files.Commands.DeleteFile
{
    public class ProvisionFileCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class ProvisionFileCommandHandler : IRequestHandler<ProvisionFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public ProvisionFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ProvisionFileCommand request, CancellationToken cancellationToken)
        {


            var file = await _context.Files
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (file == null)
            {
                throw new NotFoundException(nameof(File), request.Id);
            }

            long fileTimeSpan = (file.EndTime - file.StartTime).Ticks;
            long sliceDuration = TimeSpan.FromSeconds(600).Ticks; //Value From Configuration
            int numberOfSlicesToGenerate = (int)(fileTimeSpan / sliceDuration);

            Job job = new Job
            {
                FileId = request.Id,
                Status = 0,
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync(new CancellationToken());

            Slice slice;

            var startTime = new DateTime(file.StartTime.Ticks);
            var endTime = new DateTime(file.StartTime.Ticks + sliceDuration);

            for (int i = 1; i < numberOfSlicesToGenerate + 1; i++)
            {
                slice = new Slice
                {
                    JobId = job.Id,
                    Status = Domain.Enums.SliceStatus.Pending,
                    StartTime = startTime,
                    EndTime = endTime
                };

                _context.Slices.Add(slice);

                startTime = new DateTime(endTime.Ticks + 1);
                endTime = new DateTime(file.StartTime.Ticks + sliceDuration);
            }

            file.Status = Domain.Enums.FileStatus.JobProvisioned;
            await _context.SaveChangesAsync(new CancellationToken());

            return Unit.Value;
        }
    }
}
