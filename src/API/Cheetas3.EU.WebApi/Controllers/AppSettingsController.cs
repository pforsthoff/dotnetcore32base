using Cheetas3.EU.Application.Features.Parameters.Commands.UpdateParameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cheetas3.EU.Application.Features.AppSettings.Queries;

namespace Cheetas3.EU.Controllers
{

    public class AppSettingsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<AppSettingsDto> Get()
        {
            return await Mediator.Send(new GetAppSettingsQuery());
        }

        [HttpPost("updateappsettings")]
        public async Task<ActionResult> UpdateAppSettingsAsync(UpdateAppSettingsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
