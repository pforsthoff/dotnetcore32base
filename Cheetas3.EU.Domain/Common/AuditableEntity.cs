using System;

namespace Cheetas3.EU.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreationDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
