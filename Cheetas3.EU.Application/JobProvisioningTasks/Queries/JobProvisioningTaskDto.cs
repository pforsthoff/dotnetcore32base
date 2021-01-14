using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using Cheetas3.EU.Domain.Common;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Application.JobProvisioningTasks.Queries
{
    public class JobProvisioningTaskDto : AuditableEntity, IMapFrom<JobProvisioningTask>
    {
        public int Id { get; set; }
        public Int64 FileTimeSpan { get; set; }
        public TaskStatus Status { get; set; }
        public string TaskStatus { get; set; }
        public DateTime? JobProvisionedDateTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobProvisioningTask, JobProvisioningTaskDto>()
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
