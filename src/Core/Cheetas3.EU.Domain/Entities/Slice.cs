using Cheetas3.EU.Domain.Enums;
using Cheetas3.EU.Domain.Entities.Base;
using System;

namespace Cheetas3.EU.Domain.Entities
{
    public class Slice : AuditableEntity
    {
        public int Id {get;set;}
        public int JobId { get; set; }
        public Job Job { get; set; }
        public SliceStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? SliceStarted { get; set; }
        public DateTime? SliceCompleted { get; set; }
    }
}
