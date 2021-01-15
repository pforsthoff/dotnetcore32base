using Cheetas3.EU.Application.Files.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Controllers
{
    public class FilesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<FileDto>> Get()
        {
            return await Mediator.Send(new GetFilesQuery());
        }

        [HttpGet("{id}")]
        public async Task<FileDto> GetById(int id)
        {
            return await Mediator.Send(new GetFileByIdQuery { Id = id });
        }
    }
}
