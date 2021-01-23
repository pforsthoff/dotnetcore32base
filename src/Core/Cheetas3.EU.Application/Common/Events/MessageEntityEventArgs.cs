using Cheetas3.EU.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Events
{
    public delegate Task MessageReceivedEventHandler(object? sender, MessageEntityEventArgs<Slice> e);

    public class MessageEntityEventArgs<T> : EventArgs
    {
        public T Entity { get; set; }
    }
}
