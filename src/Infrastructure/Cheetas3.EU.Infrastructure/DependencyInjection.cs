using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Identity;
using Cheetas3.EU.Persistance;
using Cheetas3.EU.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cheetas3.EU.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var cstr = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(opt => opt
#if DEBUG
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
#endif
                .UseSqlServer(cstr));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            //services.AddScoped<IRepository, Repository>();

            //services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(cstr));
            //UpgradeDatabase(app);
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseInMemoryDatabase("DemoDb"));

            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<GlobalDbContext>(options =>
            //        options.UseInMemoryDatabase("Cheetas3GlobalDb"));
            //}
            //else
            //{
            //    //services.AddDbContext<ApplicationDbContext>(options =>
            //    //    options.UseSqlServer(
            //    //        configuration.GetConnectionString("DefaultConnection"),
            //    //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            //    services.AddDbContext<GlobalDbContext>(options =>
            //        options.UseSqlServer(
            //            configuration.GetConnectionString("GlobalDB")
            //        )
            //    );
            //}

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            //services.AddScoped<IGlobalDbContext>(provider => provider.GetService<GlobalDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
                //.AddEntityFrameworkStores<GlobalDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            //.AddApiAuthorization<ApplicationUser, GlobalDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            //services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options => {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
        private static void UpgradeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}

