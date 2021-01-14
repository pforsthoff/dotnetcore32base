using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Cheetas3.EU.Application.Jobs.Queries
{
    public class GetJobByIdQuery : IRequest<JobDto>
    {
        public int Id { get; set; }
    }

    public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .Where(x => x.Id == request.Id)
                .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

        }
    }
}
