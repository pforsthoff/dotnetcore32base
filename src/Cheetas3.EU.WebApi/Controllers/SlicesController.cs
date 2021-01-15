using Cheetas3.EU.Application.Slices.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Controllers
{
    public class SlicesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<SliceDto>> Get()
        {
            return await Mediator.Send(new GetSlicesQuery());
        }

        [HttpGet("{id}")]
        public async Task<SliceDto> GetById(int id)
        {
            return await Mediator.Send(new GetSliceByIdQuery { Id = id });
        }
    }
}