﻿using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Domain.Events;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Jobs.Comands.CreateJob
{
    public class CreateJobCommand : IRequest<int>
    {
        public DateTime DateTimeJobRcvd { get; set; }
        public Int64 TimeSpan { get; set; }
        public JobStatus Status { get; set; }
    }

    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateJobCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var entity = new Job
            {
                DateTimeJobRcvd = request.DateTimeJobRcvd,
                TimeSpan = request.TimeSpan,
                Status = JobStatus.Received
            };

            //entity.DomainEvents.Add(new JobCreatedEvent(entity));

            _context.Jobs.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
