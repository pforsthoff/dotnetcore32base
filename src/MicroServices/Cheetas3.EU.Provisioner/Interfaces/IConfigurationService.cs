using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Cheetas3.EU.Provisioner.Interfaces
{
    public interface IConfigurationService
    {
        public IConfiguration Configuration { get; }
        public string ServiceHealthEndPoint { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }

        public int MaxConcurrency { get; }
        public int JobPollingFrequency { get; }
    }
}
