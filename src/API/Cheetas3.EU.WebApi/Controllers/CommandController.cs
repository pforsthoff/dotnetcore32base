using Microsoft.AspNetCore.Mvc;

namespace Cheetas3.EU.Controllers
{

    public class CommandController : ApiControllerBase
    {
        [HttpGet("createjob/{id}")]
        public ActionResult CreateJob(int id)
        {
            return Content($"Creating Conversion Job for id {id}");
        }

        [HttpGet("executejob/{id}")]
        public ActionResult ExecuteJob(int id)
        {
            return Content($"Executing Conversion Job for id {id}");
        }
    }
}
