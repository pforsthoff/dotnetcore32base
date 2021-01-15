using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Jobs.Queries
{
    public class GetJobsQuery : IRequest<IEnumerable<JobDto>>
    {
    }

    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<JobDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            return  await _context.Jobs
                .OrderBy(o => o.CreationDateTime)
                .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
