using Microsoft.Extensions.Configuration;
using System;

namespace Cheetas3.EU.Converter.Interfaces
{
    public interface IMessageQueueService
    {
        IConfiguration Configuration { get; }

        void PublishMessage(Byte[] message);
    }
}
