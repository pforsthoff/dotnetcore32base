﻿using System.Collections.Generic;


namespace Cheetas3.EU.Application.Files.Queries
{
    public class FileListDto
    {
        public FileListDto()
        {
            Files = new List<FileDto>();
        }

        public IList<FileDto> Files { get; set; }
    }
}