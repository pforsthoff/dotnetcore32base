using Cheetas3.EU.Application.Common.Interfaces;
using Steeltoe.Management.Info;


namespace Cheetas3.EU.Actuators
{
    public class ProvisioningServiceInfoContributor : IInfoContributor
    {
        private readonly IConfigurationProvisioningService _configurationService;

        public ProvisioningServiceInfoContributor(IConfigurationProvisioningService configurationService)
        {
            _configurationService = configurationService;
        }
        public void Contribute(IInfoBuilder builder)
        {
            builder.WithInfo("ProvisionerService", new {
                //status = _configurationService.ServiceInfoStatus.ToString(),
                MaxConcurrency = _configurationService.MaxConcurrency,
                SliceDurationInSeconds = _configurationService.SliceDurationInSeconds,
                DevAttributeContainerLifeDuration = _configurationService.DevAttributeContainerLifeDuration,
            });
        }
    }
}
