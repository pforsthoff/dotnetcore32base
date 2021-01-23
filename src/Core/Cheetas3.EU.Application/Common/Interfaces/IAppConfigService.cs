using Cheetas3.EU.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IAppConfigService
    {
        public IConfiguration Configuration { get; }
        public ServiceInfoStatus ServiceInfoStatus { get; set; }
        public string DockerHostUrl { get; set; }
        public int MaxConcurrency { get; set; }
        public int SliceTimeSpan { get; set; }
        public int DevAttributeContainerLifeDuration { get; set; }
        public int RetryCount { get; set; }
        public string Image { get; set; }
    }
}
