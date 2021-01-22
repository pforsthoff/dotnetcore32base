using Cheetas3.EU.Application.Common.Events;
using System;
namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IMessageQueueService
    {

        event MessageReceivedEventHandler MessageReceivedEventHandler;

        void ConsumeMessage();
        void PublishMessage(string message);
    }
}
