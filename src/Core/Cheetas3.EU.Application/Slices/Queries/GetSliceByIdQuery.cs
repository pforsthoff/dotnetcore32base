using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Cheetas3.EU.Application.Slices.Queries
{
    public class GetSliceByIdQuery : IRequest<SliceDto>
    {
        public int Id { get; set; }
    }

    public class GetSliceByIdQueryHandler : IRequestHandler<GetSliceByIdQuery, SliceDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSliceByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SliceDto> Handle(GetSliceByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Slices
                .Where(x => x.Id == request.Id)
                .ProjectTo<SliceDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

        }
    }
}
