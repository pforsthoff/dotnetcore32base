using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cheetas3.EU.Converter.Entities;
using Cheetas3.EU.Converter.Entities.Base;
using Cheetas3.EU.Converter.Interfaces;

namespace Cheetas3.EU.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    //IOptions<OperationalStoreOptions> operationalStoreOptions,
                                    IDateTime dateTime) : base(options)
        {
            _dateTime = dateTime;
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Slice> Slices { get; set; }
        public DbSet<File> Files { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "EUConverterService";
                        entry.Entity.CreationDateTime = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "EUConverterService";
                        entry.Entity.LastModifiedDateTime = _dateTime.Now;
                        break;
                }
            }

           return  await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Required if using ASP.NET Identity
            //base.OnModelCreating(builder);
        }
    }
}
