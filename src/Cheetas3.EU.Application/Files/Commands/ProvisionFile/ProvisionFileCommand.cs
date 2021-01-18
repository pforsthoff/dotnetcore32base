using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Files.Commands.ProvisionFile
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
                throw new NotFoundException(nameof(File), request.Id);


            var fileTimeSpan = (file.EndTime - file.StartTime).TotalMinutes;
            var sliceTimeSpan = 600; //Value From Configuration
            var sliceCount = (fileTimeSpan * 60) / sliceTimeSpan;

            Job job = new Job
            {
                FileId = request.Id,
                Status = 0,
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync(new CancellationToken());

            Slice slice;

            var sliceStartTime = file.StartTime;
            var sliceEndTime = sliceStartTime.AddSeconds(sliceTimeSpan);

            for (int i = 1; i < sliceCount + 1; i++)
            {
                slice = new Slice
                {
                    JobId = job.Id,
                    Status = Domain.Enums.SliceStatus.Pending,
                    StartTime = sliceStartTime,
                    EndTime = sliceEndTime
                };

                _context.Slices.Add(slice);

                sliceStartTime = sliceEndTime.AddSeconds(1);
                sliceEndTime = sliceStartTime.AddSeconds(sliceTimeSpan);

                if (i == sliceCount)
                    slice.EndTime = slice.EndTime.AddSeconds(-sliceCount + 1);
            }

            file.Status = Domain.Enums.FileStatus.JobProvisioned;
            await _context.SaveChangesAsync(new CancellationToken());

            return Unit.Value;
        }


        public double GetTimeSpan(DateTime startTime, DateTime endTime)
        {
            var span = (endTime - startTime).TotalMinutes;
            return span;
        }
    }
}
