using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using Cheetas3.EU.Domain.Entities.Base;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Application.Files.Queries
{
    public class FileDto : AuditableEntity, IMapFrom<File>
    {
        public int Id { get; set; }
        public FileStatus Status { get; set; }
        public string FileStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<File, FileDto>()
                .ForMember(dest => dest.FileStatus, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
