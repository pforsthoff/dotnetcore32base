using Cheetas3.EU.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Job> Jobs { get; set; }
        DbSet<Slice> Slices { get; set; }
        DbSet<JobProvisioningTask> JobProvisioningTasks { get;set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
