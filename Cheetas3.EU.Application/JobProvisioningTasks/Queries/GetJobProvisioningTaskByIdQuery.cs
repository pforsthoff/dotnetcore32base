using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Cheetas3.EU.Application.JobProvisioningTasks.Queries
{
    public class GetJobProvisioningTaskByIdQuery : IRequest<JobProvisioningTaskDto>
    {
        public int Id { get; set; }
    }

    public class GetTaskByIdQueryHandler : IRequestHandler<GetJobProvisioningTaskByIdQuery, JobProvisioningTaskDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTaskByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobProvisioningTaskDto> Handle(GetJobProvisioningTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.JobProvisioningTasks
                .Where(x => x.Id == request.Id)
                .ProjectTo<JobProvisioningTaskDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

        }
    }
}
