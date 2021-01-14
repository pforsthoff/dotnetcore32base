﻿using Cheetas3.EU.Domain.Common;
using Cheetas3.EU.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Cheetas3.EU.Domain.Entities
{
    public class Job : AuditableEntity
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public DateTime DateTimeJobRcvd { get; set; }
        public Int64 TimeSpan { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? DateTimeJobStarted { get; set; }
        public DateTime? DateTimeJobCompleted { get; set; }
        public virtual ICollection<Slice> Slices { get; private set; } = new List<Slice>();
    }
}
