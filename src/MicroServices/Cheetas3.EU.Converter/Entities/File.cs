using Cheetas3.EU.Converter.Entities.Base;
using Cheetas3.EU.Converter.Enums;
using System;

namespace Cheetas3.EU.Converter.Entities
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
