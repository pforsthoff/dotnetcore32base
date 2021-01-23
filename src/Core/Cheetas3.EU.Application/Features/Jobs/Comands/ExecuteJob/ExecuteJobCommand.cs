using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cheetas3.EU.Application.Common.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using k8s.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Cheetas3.EU.Application.Features.Jobs.Comands.ExecuteJob
{
    public class ExecuteJobCommand : IRequest<JobStatus>
    {
        public int Id { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
    }

    public class ExecuteJobCommandHandler : IRequestHandler<ExecuteJobCommand, JobStatus>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJobService _jobService;
        private readonly IDateTime _dateTime;
        private readonly ILogger<ExecuteJobCommandHandler> _logger;

        public ExecuteJobCommandHandler(IApplicationDbContext context, IJobService jobService, IDateTime dateTime, ILogger<ExecuteJobCommandHandler> logger)
        {
            _context = context;
            _jobService = jobService;
            _dateTime = dateTime;
            _logger = logger;
        }

        public async Task<JobStatus> Handle(ExecuteJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.Include(j => j.Slices)
                 .Where(p => p.Id == request.Id && p.Status != JobStatus.Completed)
                 .SingleOrDefaultAsync(cancellationToken);

            if (job == null)
                throw new NotFoundException(nameof(Job), request.Id);

            job.Status = JobStatus.InProgress;
            job.StartedDateTime = _dateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);

            return await _jobService.ProcessJob(job, request.TargetPlatform);
        }
    }
}
