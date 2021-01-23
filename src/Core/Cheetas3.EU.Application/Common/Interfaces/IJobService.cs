using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IJobService
    {
        Task<JobStatus> ProcessJob(Job job, TargetPlatform platform);
    }
}
