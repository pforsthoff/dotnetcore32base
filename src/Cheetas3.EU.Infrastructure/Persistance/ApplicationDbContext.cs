using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Application.Interfaces;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Entities.Base;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Cheetas3.EU.Domain.Events;
using Cheetas3.EU.Infrastructure.Identity;

namespace Cheetas3.EU.Infrastructure.Persistance
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        //private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IOptions<OperationalStoreOptions> operationalStoreOptions,
                                    //IDomainEventService domainEventService,
                                    IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            //_domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Slice> Slices { get; set; }
        public DbSet<File> Files { get; set; }

        //public DbSet<Slices> Slices { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedBy = "Admin";
                        entry.Entity.CreationDateTime = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModifiedBy = "Admin";
                        entry.Entity.LastModifiedDateTime = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            //await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //Required if using ASP.NET Identity
            base.OnModelCreating(builder);
        }
        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .FirstOrDefault();
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}
