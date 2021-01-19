using Microsoft.AspNetCore.Mvc;

namespace Cheetas3.EU.Controllers
{

    public class CommandsController : ApiControllerBase
    {
        [HttpGet("createjob/{id}")]
        public ActionResult CreateJob(int id)
        {
            return Content($"Creating Conversion Job for id {id}");
        }

        //[Route("api/[controller]/createjob")]
        [HttpPost("ExecuteJob")]
        public ActionResult ExecuteJob(int id)
        {
            return Content($"Executing Conversion Job for id {id}");
        }
    }
}
