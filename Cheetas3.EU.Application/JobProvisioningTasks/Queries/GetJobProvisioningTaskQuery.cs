using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.JobProvisioningTasks.Queries
{
    public class GetJobProvisioningTasksQuery : IRequest<IEnumerable<JobProvisioningTaskDto>>
    {
    }

    public class GetJobProvisioningTasksQueryHandler : IRequestHandler<GetJobProvisioningTasksQuery, IEnumerable<JobProvisioningTaskDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobProvisioningTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobProvisioningTaskDto>> Handle(GetJobProvisioningTasksQuery request, CancellationToken cancellationToken)
        {
            return  await _context.JobProvisioningTasks
                .OrderBy(o => o.CreationDateTime)
                .ProjectTo<JobProvisioningTaskDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
