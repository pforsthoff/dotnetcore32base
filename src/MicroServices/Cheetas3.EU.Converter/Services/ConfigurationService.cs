using Cheetas3.EU.Converter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Cheetas3.EU.Domain.Enums;

namespace Cheetas3.EU.Converter.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ILogger<ConfigurationService> _logger;
        public IConfiguration Configuration { get; }
        public string ServiceHealthEndPoint { get; }
        public string Status { get; set; }
        public int SliceId { get; set; }
        public int SleepDuration { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public int JobId { get; set; }
        public int Id { get; set; }
        public int SliceCount { get; set; }

        //public ConfigurationService(IConfiguration configuration)
        public ConfigurationService(IConfiguration configuration, ILogger<ConfigurationService> logger)
        {
            Configuration = configuration;
            _logger = logger;

            ServiceHealthEndPoint = Configuration.GetValue<string>("ServiceHealthEndPoint");
            SliceId = Configuration.GetValue<int>("SliceId");
            SleepDuration = Configuration.GetValue<int>("SleepDuration");
            _logger.LogInformation("Configuration Service Started");
        }
    }
}
