﻿using Cheetas3.EU.Application.Features.Parameters.Commands.UpdateParameters;
using Microsoft.AspNetCore.Mvc;

namespace Cheetas3.EU.Controllers
{

    public class ParametersController : ApiControllerBase
    {
        [HttpPost("updateparameters")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateParametersAsync(int maxConcurrency, int devAttributeContainerLifeDuration, int sliceDurationInSeconds)
        {
            await Mediator.Send(new UpdateParametersCommand
            {
                MaxConcurrency = maxConcurrency,
                DevAttributeContainerLifeDuration = devAttributeContainerLifeDuration,
                SliceDurationInSeconds = sliceDurationInSeconds
            });
            return NoContent();
        }
    }
}
