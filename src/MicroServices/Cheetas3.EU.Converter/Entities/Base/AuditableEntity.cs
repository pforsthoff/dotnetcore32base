﻿using System;

namespace Cheetas3.EU.Converter.Entities.Base
{
    public abstract class AuditableEntity : Entity
    {
        public DateTime CreationDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
