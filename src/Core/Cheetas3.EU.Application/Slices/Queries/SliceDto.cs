using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using Cheetas3.EU.Domain.Entities.Base;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Application.Slices.Queries
{
    public class SliceDto : AuditableEntity, IMapFrom<Job>
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public SliceStatus Status { get; set; }
        public string SliceStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long Duration { get; set; }
        public DateTime? SliceStarted { get; set; }
        public DateTime? SliceCompleted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Slice, SliceDto>()
                .ForMember(dest => dest.SliceStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (src.EndTime - src.StartTime).Ticks))
                .ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.Job));
        }
    }
}
