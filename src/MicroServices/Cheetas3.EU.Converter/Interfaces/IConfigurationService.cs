﻿using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Cheetas3.EU.Converter.Interfaces
{
    public interface IConfigurationService
    {
        public IConfiguration Configuration { get; }
        public string ServiceHealthEndPoint { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public int SliceId { get; set; }
        public int JobId { get; set; }
        public int SliceCount { get; set; }
        public int Id { get; set; }
        public int SleepDuration { get; }
    }
}