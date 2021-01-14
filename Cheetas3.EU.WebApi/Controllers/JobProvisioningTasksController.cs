using Cheetas3.EU.Application.JobProvisioningTasks.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Controllers
{
    public class JobProvisioningTasksController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<JobProvisioningTaskDto>> Get()
        {
            return await Mediator.Send(new GetJobProvisioningTasksQuery());
        }

        [HttpGet("{id}")]
        public async Task<JobProvisioningTaskDto> GetById(int id)
        {
            return await Mediator.Send(new GetJobProvisioningTaskByIdQuery { Id = id });
        }


    }
}
