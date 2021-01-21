using System;
namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IMessageQueueService
    {
        //event EventHandler MessageReceived;
        void ConsumeMessage();
        void PublishMessage(string message);
    }
}
