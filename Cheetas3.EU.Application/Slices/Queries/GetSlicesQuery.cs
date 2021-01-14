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
    public class GetSlicesQuery : IRequest<IEnumerable<SliceDto>>
    {
    }

    public class GetSlicesQueryHandler : IRequestHandler<GetSlicesQuery, IEnumerable<SliceDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSlicesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SliceDto>> Handle(GetSlicesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Slices
                .OrderBy(o => o.CreationDateTime)
                .ProjectTo<SliceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
