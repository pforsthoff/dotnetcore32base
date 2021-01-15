using Cheetas3.EU.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cheetas3.EU.Infrastructure.Persistance.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("Files");
            builder.Property(p => p.StartTime).IsRequired();
            builder.Property(p => p.EndTime).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            //builder.HasOne(p => p.Job)
            //.WithOne(b => b.File)
            //.HasForeignKey(fk => fk.Job);
        }
    }
}
