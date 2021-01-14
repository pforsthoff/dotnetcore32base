using Cheetas3.EU.Domain.Common;
using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Domain.Entities
{
    public class JobProvisioningTask : AuditableEntity
    {
        public int Id { get; set; }
        public Int64 FileTimeSpan { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? JobProvisionedDateTime { get; set; }
    }
}
