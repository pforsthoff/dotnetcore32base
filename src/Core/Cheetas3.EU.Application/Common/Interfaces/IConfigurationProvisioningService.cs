using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IConfigurationProvisioningService
    {
        public IConfiguration Configuration { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public int MaxConcurrency { get; set; }
        public int SliceDurationInSeconds { get; set; }
        public int DevAttributeContainerLifeDuration { get; set; }
    }
}
