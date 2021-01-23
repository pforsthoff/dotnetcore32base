using Cheetas3.EU.Application.Features.Jobs.Comands.ExecuteJob;
using Cheetas3.EU.Application.Features.Jobs.Queries;
using Cheetas3.EU.Domain.Enums;
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

        [HttpPost("ExecuteJob")]
        public async Task<ActionResult> ExecuteJob(int id, TargetPlatform targetPlatform)
        //public async Task<ActionResult> ExecuteJob(ExecuteJobCommand command)
        {

            await Mediator.Send(new ExecuteJobCommand { Id = id, TargetPlatform = targetPlatform });
            //await Mediator.Send(command);
            return NoContent();
        }
    }
}