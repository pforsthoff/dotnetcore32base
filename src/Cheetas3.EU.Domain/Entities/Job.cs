using Cheetas3.EU.Domain.Entities.Base;
using Cheetas3.EU.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Cheetas3.EU.Domain.Entities
{
    public class Job : AuditableEntity
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? StartedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public virtual ICollection<Slice> Slices { get; private set; } = new List<Slice>();
    }
}
