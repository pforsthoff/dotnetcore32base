using Cheetas3.EU.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cheetas3.EU.Infrastructure.Persistance.Mappings
{
    public class SliceConfiguration : IEntityTypeConfiguration<Slice>
    {
        public void Configure(EntityTypeBuilder<Slice> builder)
        {
            builder.ToTable("Slices");
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.JobId).IsRequired();
        }
    }
}
