using Cheetas3.EU.Converter.Entities.Base;
using Cheetas3.EU.Converter.Enums;
using System;

namespace Cheetas3.EU.Converter.Entities
{
    public class Slice : AuditableEntity
    {
        public int Id {get;set;}
        public int JobId { get; set; }
        public Job Job { get; set; }
        public SliceStatus Status { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? SliceStarted { get; set; }
        public DateTime? SliceCompleted { get; set; }
    }
}
