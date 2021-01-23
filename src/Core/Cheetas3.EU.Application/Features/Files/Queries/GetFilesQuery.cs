using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Features.Files.Queries
{
    public class GetFilesQuery : IRequest<IEnumerable<FileDto>>
    {
    }

    public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, IEnumerable<FileDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFilesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FileDto>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            return  await _context.Files
                .OrderBy(o => o.CreationDateTime)
                .ProjectTo<FileDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
