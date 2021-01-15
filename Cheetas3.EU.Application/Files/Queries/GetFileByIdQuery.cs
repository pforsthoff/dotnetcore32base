using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Cheetas3.EU.Application.Files.Queries
{
    public class GetFileByIdQuery : IRequest<FileDto>
    {
        public int Id { get; set; }
    }

    public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, FileDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFileByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FileDto> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Files
                .Where(x => x.Id == request.Id)
                .ProjectTo<FileDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

        }
    }
}
