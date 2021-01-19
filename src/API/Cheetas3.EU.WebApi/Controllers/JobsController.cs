using Cheetas3.EU.Application.Jobs.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Controllers
{
    public class JobsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<JobDto>> Get()
        {
            return await Mediator.Send(new GetJobsQuery());
        }

        [HttpGet("{id}")]
        public async Task<JobDto> GetById(int id)
        {
            return await Mediator.Send(new GetJobByIdQuery { Id = id });
        }
    }
}