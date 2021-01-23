using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Cheetas3.EU.Converter.Interfaces;
using Cheetas3.EU.Converter.Entities;
using Cheetas3.EU.Converter.Enums;
using Cheetas3.EU.Converter.Extensions;
using Cheetas3.EU.Converter.Exceptions;

namespace Cheetas3.EU.Converter.Services
{
    public class ConversionService : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IConfigurationService _configurationService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<ConversionService> _logger;
        private readonly IApplicationDbContext _context;
        private Timer _timer;
        private Slice _slice;
        private bool _continuePolling = true;

        public ConversionService(IConfigurationService configurationService,
                                 IMessageQueueService messageQueueService,
                                 ILogger<ConversionService> logger,
                                 IApplicationDbContext context,
                                 IHostApplicationLifetime applicationLifetime)
        {
            _configurationService = configurationService;
            _messageQueueService = messageQueueService;
            _logger = logger;
            _context = context;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Conversion Service is Initializaing for SliceId: {_configurationService.SliceId}");

            //Poll Container Health Every 5 Seconds
            _timer = new Timer(PollServiceHealthStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void PollServiceHealthStatus(object state)
        {
            var status = GetServiceHealthStatus();

            if (status == ServiceHealthStatus.Up && _continuePolling)
            {
                _timer.Dispose();
                _continuePolling = false;
                DoConversion();
                ShutDownApp();
            }
        }

        private void ShutDownApp()
        {
            _logger.LogInformation($"Conversion completed for JobId:{_slice.JobId}, SliceId:{_slice.Id}");
            _logger.LogInformation("Shutting down application");
            StopAsync(new CancellationToken());
            _applicationLifetime.StopApplication();
        }

        private void UpdateConfigurationServiceProperties(Slice slice)
        {
            _configurationService.ServiceInfoStatus = ServiceInfoStatus.Running;
            _configurationService.JobId = slice.JobId;
            _configurationService.Id = slice.Id;
            _configurationService.SliceCount = slice.Job.Slices.Count;

            _logger.LogInformation("Updated Configuration Service Properties");
        }
        private void DoConversion()
        {
            //If 0 is passed into config.service, we're not doing any updates
            _slice = _context.Slices
                .Where(x => x.Id == _configurationService.SliceId).Include(i => i.Job.Slices)
                .SingleOrDefault();

            _logger.LogInformation($"Found slice with JobId:{_slice.JobId}, SliceId:{_slice.Id} with status of {_slice.Status} to convert");

            UpdateConfigurationServiceProperties(_slice);

            if (_slice.Status == SliceStatus.Pending)
            {
                _slice.Status = SliceStatus.Running;
                _slice.SliceStarted = DateTime.Now;
                _messageQueueService.PublishMessage(_slice.ToMessage());


                _logger.LogInformation($"SliceId {_slice.Id} conversion has started.");
                _configurationService.ServiceInfoStatus = ServiceInfoStatus.Waiting;
                Thread.Sleep(_configurationService.SleepDuration);

                for (int i = 0; i < 10; i++)
                {
                    _configurationService.Status += ".";
                    Thread.Sleep(_configurationService.SleepDuration/10);
                }
                _configurationService.ServiceInfoStatus = ServiceInfoStatus.Running;
                //Complete Slice Conversion
                _slice.Status = SliceStatus.Completed;
                _slice.SliceCompleted = DateTime.Now;
                _messageQueueService.PublishMessage(_slice.ToMessage());

                _configurationService.ServiceInfoStatus = ServiceInfoStatus.CompletedSuccessfully;
                _logger.LogInformation($"SliceId {_slice.Id} conversion has completed.");

                _configurationService.ServiceInfoStatus = ServiceInfoStatus.CompletedSuccessfully;

            }
            else
            {
                _logger.LogError($"SliceId {_slice.Id} is either currently running or already completed.");
            }
        }

        private ServiceHealthStatus GetServiceHealthStatus()
        {
            var status = ServiceHealthStatus.Down;
            var webRequest = WebRequest.Create(_configurationService.ServiceHealthEndPoint);
            int retryCountAttempts = 0;
            int retryCount = _configurationService.RetryCount;
            try
            {
                var stream = webRequest.GetResponse().GetResponseStream();

                var reader = new StreamReader(stream);
                var rawJson = reader.ReadToEnd();
                var json = JObject.Parse(rawJson);
                var doc = (JContainer)json["details"];
                var results = doc.Descendants()
                    .OfType<JObject>()
                    .Where(x => x["status"] != null &&
                                x["status"].Value<string>() == "DOWN");

                //Root Health Status Indications
                status = (ServiceHealthStatus)Enum.Parse(typeof(ServiceHealthStatus),json.GetValue("status").ToString(), true) ;
                if (results.Any() || status == ServiceHealthStatus.Down)
                {
                    status = ServiceHealthStatus.Down;

                    string parent = string.Empty;
                    string description = string.Empty;
                    string errorMessage = string.Empty;
                    foreach (var result in results)
                    {
                        parent = ((JProperty)result.Parent).Name;
                        description = result.GetValue("description").Value<string>();
                        errorMessage = $"Service Health is a Reporting Down Status for {parent} with a failure of {description}.";
                    }

                    _logger.LogError(errorMessage);
                    throw new ServiceDownException(errorMessage);
                }
            }
            catch (ServiceDownException ex)
            {
                //Retry
                retryCountAttempts++;
                _logger.LogError($"Service Health is a Reporting Down Status, exception iteration:{retryCount}.", ex);

                if (retryCount > retryCountAttempts)
                {
                    _logger.LogError($"Too many retries, stopping service.");
                    StopAsync(new CancellationToken());
                }
                return status;
            }

            return status;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Conversion Service is stopping.");
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
