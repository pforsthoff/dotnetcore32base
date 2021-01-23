using Cheetas3.EU.Converter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Cheetas3.EU.Converter.Enums;

namespace Cheetas3.EU.Converter.Services
{

    public class AppConfigService : IAppConfigService
    {
        public IConfiguration Configuration { get; }
        public int Id { get; set; }
        public int SliceId { get; set; }
        public int JobId { get; set; }
        public int SliceCount { get; set; }
        public string ServiceHealthEndPoint { get; set; }
        public int RetryCount { get; set; } = 5;
        public int SleepDuration { get; set; } = 300000;
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public TargetPlatform ConverterPlatform { get; set; }

        public AppConfigService(IConfiguration configuration, ILogger<AppConfigService> logger)
        {
            Configuration = configuration;
            ServiceHealthEndPoint = "http://localhost:5000/actuator/health";

            TargetPlatform converterPlatform = Configuration.GetValue<TargetPlatform>("ConverterPlatform");
            if (ConverterPlatform != Configuration.GetValue<TargetPlatform>("AppSettings:ConverterPlatform"))
            {
                ConverterPlatform = converterPlatform;
                logger.LogInformation($"ConverterPlatform Passed into Service. Value:{converterPlatform}");
            }

            int sleepDuration = Configuration.GetValue<int>("SleepDuration");
            if (sleepDuration != 0)
            {
                SleepDuration = sleepDuration;
                logger.LogInformation($"SleepDuration Passed into Service. Value:{sleepDuration}");
            }

            int retryCount = Configuration.GetValue<int>("RetryCount");
            if (retryCount != 0)
            {
                RetryCount = retryCount;
                logger.LogInformation($"RetryCount Passed into Service. Value:{retryCount}");
            }

            SliceId = Configuration.GetValue<int>("SliceId");
            logger.LogInformation("***** Configuration Service Started with following properties *****");
            logger.LogInformation($"SliceId:{SliceId},SleepDurationValue:{SleepDuration},RetryCount:{RetryCount}, ConverterPlatform:{ConverterPlatform}");
        }
    }
}
