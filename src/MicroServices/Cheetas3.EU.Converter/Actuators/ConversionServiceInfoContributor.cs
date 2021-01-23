using Cheetas3.EU.Converter.Interfaces;
using Steeltoe.Management.Info;

namespace Cheetas3.EU.Converter.Actuators
{
    public class ConversionServiceInfoContributor : IInfoContributor
    {
        private readonly IConfigurationService _configurationService;

        public ConversionServiceInfoContributor(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        public void Contribute(IInfoBuilder builder)
        {
            builder.WithInfo("ConverterService", new {
                status = _configurationService.ServiceInfoStatus.ToString(),
                workstatus = _configurationService.Status,
                sleepDuration = _configurationService.SleepDuration,
                apiUrl = _configurationService.ServiceHealthEndPoint,
                jobId = _configurationService.JobId,
                sliceId = _configurationService.SliceId,
                jobstatus = $"Processing Slice { _configurationService.Id } of {_configurationService.SliceCount} slices for provision job {_configurationService.JobId}."
            });
        }
    }
}
