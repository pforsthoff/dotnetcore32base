using Cheetas3.EU.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cheetas3.EU.Infrastructure.Persistance.Configurations
{
    public class JobProvisioningTaskConfiguration : IEntityTypeConfiguration<JobProvisioningTask>
    {
        public void Configure(EntityTypeBuilder<JobProvisioningTask> builder)
        {
            builder.ToTable("JobProvisioningTasks");
            builder.Property(t => t.FileTimeSpan).IsRequired();
            builder.Property(t => t.Status).IsRequired();
        }
    }
}
