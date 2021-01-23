using Cheetas3.EU.Converter.Interfaces;
using Steeltoe.Management.Info;

namespace Cheetas3.EU.Converter.Actuators
{
    public class ConversionServiceInfoContributor : IInfoContributor
    {
        private readonly IAppConfigService _appConfigService;

        public ConversionServiceInfoContributor(IAppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
        }
        public void Contribute(IInfoBuilder builder)
        {
            builder.WithInfo("ConverterService", new {
                status = _appConfigService.ServiceInfoStatus.ToString(),
                coverterPlatform = _appConfigService.ConverterPlatform.ToString(),
                sleepDuration = _appConfigService.SleepDuration,
                retryCount = _appConfigService.RetryCount,
                apiInfoUrl = "http://localhost:5000/actuator/info",
                apiHealthUrl = "http://localhost:5000/actuator,health",
                jobId = _appConfigService.JobId,
                sliceId = _appConfigService.SliceId,
                jobstatus = $"Processing Slice { _appConfigService.Id } " +
                    $"of {_appConfigService.SliceCount} " +
                    $"slices for provision job {_appConfigService.JobId}. "
            });
        }
    }
}
