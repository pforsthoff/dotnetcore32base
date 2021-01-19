using Cheetas3.EU.Domain.Enums;
using System;

namespace Cheetas3.EU.Models
{
    public class JobSliceModel
    {
        public int ConversionJobId { get; set; }
        public int SliceId { get; set; }
        public SliceStatus Status { get; set; }
        public string SliceStatus { get; set; }
        public DateTime? SliceStarted { get; set; }
        public DateTime? SliceCompleted { get; set; }
    }
}
