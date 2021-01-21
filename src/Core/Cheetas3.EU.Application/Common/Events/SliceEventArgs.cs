using Cheetas3.EU.Domain.Entities;
using System;

namespace Cheetas3.EU.Application.Common.Events
{
    public class SliceEventArgs : EventArgs
    {
        public Slice Slice { get; set; }
    }
}
