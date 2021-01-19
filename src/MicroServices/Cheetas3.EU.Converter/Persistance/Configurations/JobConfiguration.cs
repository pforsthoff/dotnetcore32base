using Cheetas3.EU.Converter.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cheetas3.EU.Converter.Persistance.Configurations
{
    public class JobConfiguration: IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs");
            builder.Property(p => p.Status).IsRequired();
            builder.HasMany(e => e.Slices)
                .WithOne(e => e.Job)
                .HasForeignKey(e => e.JobId);
        }
    }
}