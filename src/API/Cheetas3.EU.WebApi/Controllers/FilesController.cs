using Cheetas3.EU.Application.Files.Commands.CreateFile;
using Cheetas3.EU.Application.Files.Commands.DeleteFile;
using Cheetas3.EU.Application.Files.Commands.UpdateFile;
using Cheetas3.EU.Application.Files.Commands.ProvisionFile;
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

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateFileCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateFileCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteFileCommand { Id = id });

            return NoContent();
        }

        [HttpPost("provisionfile")]
        public async Task<ActionResult> ProvisionFile(int id, int sliceDuration)
        {
            await Mediator.Send(new ProvisionFileCommand { Id = id, SliceDuration = sliceDuration });
            return NoContent();
        }
    }
}