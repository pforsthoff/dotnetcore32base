using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using Cheetas3.EU.Domain.Common;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Cheetas3.EU.Application.Jobs.Queries
{
    public class JobDto : AuditableEntity, IMapFrom<Job>
    {
        public int Id { get; set; }
        public DateTime DateTimeJobRcvd { get; set; }
        public Int64 TimeSpan { get; set; }
        public JobStatus Status { get; set; }
        public string JobStatus { get; set; }
        public DateTime? DateTimeJobStarted { get; set; }
        public DateTime? DateTimeJobCompleted { get; set; }
        public virtual ICollection<Slice> Slices { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Job, JobDto>()
                .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Slices, opt => opt.MapFrom(src => src.Slices));
        }
    }
}
