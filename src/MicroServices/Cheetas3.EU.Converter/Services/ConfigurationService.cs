using Cheetas3.EU.Converter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Cheetas3.EU.Converter.Enums;

namespace Cheetas3.EU.Converter.Services
{

    public class ConfigurationService : IConfigurationService
    {
        private const string DEFAULT_HEALTH_ENDPOINT = "http://localhost:5000/actuator/health";

        private readonly ILogger<ConfigurationService> _logger;
        public IConfiguration Configuration { get; }
        public string ServiceHealthEndPoint { get; } = DEFAULT_HEALTH_ENDPOINT;
        public string Status { get; set; }
        public int SliceId { get; set; }
        public int SleepDuration { get; } = 300000;
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public int JobId { get; set; }
        public int Id { get; set; }
        public int SliceCount { get; set; }

        //public ConfigurationService(IConfiguration configuration)
        public ConfigurationService(IConfiguration configuration, ILogger<ConfigurationService> logger)
        {
            Configuration = configuration;
            _logger = logger;

            string serviceHealthEndPoint = Configuration.GetValue<string>("ServiceHealthEndPoint");
            if (!string.IsNullOrEmpty(serviceHealthEndPoint))
            {
                ServiceHealthEndPoint = serviceHealthEndPoint;
                _logger.LogInformation($"ServiceHealthEndPoint Passed into Service. Value:{serviceHealthEndPoint}");
            }

            int sleepDuration = Configuration.GetValue<int>("SleepDuration");
            if (sleepDuration != 0)
            {
                SleepDuration = sleepDuration;
                _logger.LogInformation($"SleepDuration Passed into Service. Value:{sleepDuration}");
            }

            SliceId = Configuration.GetValue<int>("SliceId");
            _logger.LogInformation("Configuration Service Started");
            _logger.LogInformation($"SliceId: {SliceId}, SleepDurationValue: {SleepDuration}, ServiceHealthEndPointValue: {ServiceHealthEndPoint}");
        }
    }
}
