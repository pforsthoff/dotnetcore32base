using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Slices.Queries
{
    public class GetSlicesByJobIdQuery : IRequest<IEnumerable<SliceDto>>
    {
        public int JobId { get; set; }
    }

    public class GetSlicesByJobIdQueryHandler : IRequestHandler<GetSlicesByJobIdQuery, IEnumerable<SliceDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSlicesByJobIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SliceDto>> Handle(GetSlicesByJobIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Slices
                .Where(o => o.JobId == request.JobId)
                .OrderBy(o => o.Id)
                .ProjectTo<SliceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
