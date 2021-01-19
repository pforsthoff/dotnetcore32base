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
        //Set Default Value to 600 (10 Minutes)
        public int SliceDuration { get; set; } = 600;
    }

    public class ProvisionFileCommandHandler : IRequestHandler<ProvisionFileCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileProvisioningService _fileProvisioningService;

        public ProvisionFileCommandHandler(IApplicationDbContext context, IFileProvisioningService fileProvisioningService)
        {
            _context = context;
            _fileProvisioningService = fileProvisioningService;
        }

        public async Task<Unit> Handle(ProvisionFileCommand request, CancellationToken cancellationToken)
        {

            var file = await _context.Files
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (file == null)
                throw new NotFoundException(nameof(File), request.Id);

            Job job = new Job
            {
                FileId = request.Id,
                Status = 0,
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync(new CancellationToken());


            var slices = _fileProvisioningService.CreateJobSlices(file.StartTime, file.EndTime, request.SliceDuration, job.Id);

            foreach (var slice in slices.OrderBy(o => o.StartTime))
                _context.Slices.Add(slice);

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
