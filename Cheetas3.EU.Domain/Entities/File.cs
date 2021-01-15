using Cheetas3.EU.Domain.Common;
using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Domain.Entities
{
    public class File : AuditableEntity
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FileStatus Status { get; set; }
        public DateTime? JobProvisionedDateTime { get; set; }
        public Job Job { get; set; }
    }
}
