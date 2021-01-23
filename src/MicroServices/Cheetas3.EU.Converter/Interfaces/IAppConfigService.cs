﻿using Cheetas3.EU.Converter.Enums;
using Microsoft.Extensions.Configuration;

namespace Cheetas3.EU.Converter.Interfaces
{
    public interface IAppConfigService
    {
        public IConfiguration Configuration { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public TargetPlatform ConverterPlatform { get; set; }
        public string ServiceHealthEndPoint { get; set; }
        public int RetryCount { get; set; }
        public int SliceId { get; set; }
        public int JobId { get; set; }
        public int SliceCount { get; set; }
        public int Id { get; set; }
        public int SleepDuration { get; set; }
    }
}