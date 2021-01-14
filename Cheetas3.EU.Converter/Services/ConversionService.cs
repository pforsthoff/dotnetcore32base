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
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;

namespace Cheetas3.EU.Converter.Services
{
    public class ConversionService : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<ConversionService> _logger;
        private readonly IApplicationDbContext _context;
        private Timer _timer;
        private Slice _slice;

        public ConversionService(IConfigurationService configurationService,
                                 ILogger<ConversionService> logger,
                                 IApplicationDbContext context,
                                 IHostApplicationLifetime applicationLifetime)
        {
            _configurationService = configurationService;
            _logger = logger;
            _context = context;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Conversion Service is Initializaing.");

            //Poll Container Health Every 2 Seconds
            _timer = new Timer(PollServiceHealthStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        private void PollServiceHealthStatus(object state)
        {
            var status = GetServiceHealthStatus();

            if (status == ServiceHealthStatus.Up)
            {
                _timer.Dispose();
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
            _slice = _context.Slices
                .Where(x => x.Id == _configurationService.SliceId).Include(i => i.Job.Slices)
                .SingleOrDefault();

            _logger.LogInformation($"Found slice with JobId:{_slice.JobId}, SliceId:{_slice.Id} with status of {_slice.Status} to convert");

            UpdateConfigurationServiceProperties(_slice);

            if (_slice.Status == SliceStatus.Pending)
            {
                _slice.Status = SliceStatus.Running;
                _slice.SliceStarted = DateTime.Now;
                _context.Slices.Update(_slice);
                _context.SaveChangesAsync(new CancellationToken());
                _configurationService.ServiceInfoStatus = ServiceInfoStatus.Running;
                _logger.LogInformation($"SliceId {_slice.Id} conversion has started.");

                Thread.Sleep(_configurationService.SleepDuration);

                //Complete Slice Conversion
                _slice.Status = SliceStatus.Completed;
                _slice.SliceCompleted = DateTime.Now;
                _context.Slices.Update(_slice);
                _context.SaveChangesAsync(new CancellationToken());

                _configurationService.ServiceInfoStatus = ServiceInfoStatus.CompletedSuccessfully;
                _logger.LogInformation($"SliceId {_slice.Id} conversion has completed.");
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
            int retryCount = 0;
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
                    _logger.LogError("Service Health is Reporting Down");
                }
            }
            catch (Exception)
            {
                //Retry
                retryCount++;
                _logger.LogError($"Service Health is Reporting Down, exception iteration {retryCount}.");

                if (retryCount > 4)
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
