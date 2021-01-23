using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheetas3.EU.Infrastructure.Services
{
    public class AppConfigService : IAppConfigService
    {
        public IConfiguration Configuration { get; }
        public ILogger<AppConfigService> _logger;


        public ServiceInfoStatus ServiceInfoStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int MaxConcurrency { get; set; }
        public int SliceDurationInSeconds { get; set; }
        public int DevAttributeContainerLifeDuration { get; set; }
        public string Image { get; set; }
        public int RetryCount { get; set; }



        public AppConfigService(IConfiguration configuration, ILogger<AppConfigService> logger)
        {
            Configuration = configuration;
            _logger = logger;

            var appSettings = Configuration.GetSection("AppSettings");
            var test = Configuration["AppSettings:Maxconcurrency"];

            int maxConcurrency = Configuration.GetValue<int>("AppSettings:MaxConcurrency");
            if (maxConcurrency != 0)
            {
                MaxConcurrency = maxConcurrency;
                _logger.LogInformation($"MaxConcurrency Passed into Service. Value:{maxConcurrency}");
            }

            int sliceDurationInSeconds = Configuration.GetValue<int>("AppSettings:SliceDurationInSeconds");
            if (sliceDurationInSeconds != 0)
            {
                SliceDurationInSeconds = sliceDurationInSeconds;
                _logger.LogInformation($"SliceDurationInSeconds Passed into Service. Value:{sliceDurationInSeconds}");
            }

            int devAttributeContainerLifeDuration = Configuration.GetValue<int>("AppSettings:DevAttributeContainerLifeDuration");
            if (devAttributeContainerLifeDuration != 0)
            {
                DevAttributeContainerLifeDuration = devAttributeContainerLifeDuration;
                _logger.LogInformation($"DevAttributeContainerLifeDuration Passed into Service. Value:{devAttributeContainerLifeDuration}");
            }

            string image = Configuration.GetValue<string>("AppSettings:Image");
            if (image != string.Empty)
            {
                Image = image;
                _logger.LogInformation($"Image Passed into Service. Value:{image}");
            }

            int retryCount = Configuration.GetValue<int>("AppSettings:RetryCount");
            if (retryCount != 0)
            {
                RetryCount = retryCount;
                _logger.LogInformation($"RetryCount Passed into Service. Value:{retryCount}");
            }

            _logger.LogInformation("Configuration Service Started");
        }
    }
}
