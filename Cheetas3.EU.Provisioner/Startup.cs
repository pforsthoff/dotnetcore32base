using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Info;
using Cheetas3.EU.Converter.Actuators;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Cheetas3.EU.Converter.Services;
using Cheetas3.EU.Converter.Interfaces;
using Cheetas3.EU.Infrastructure.Persistance;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Infrastructure;

namespace Cheetas3.EU.Converter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddInfrastructure(Configuration);

            services.AddSingleton<IConfigurationService, ConfigurationService>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddHealthActuator(Configuration);
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();
            services.AddInfoActuator(Configuration);
            services.AddSingleton<IInfoContributor, ConversionServiceInfoContributor>();

            //Sql Server Database Stuff
            //var cstr = Configuration.GetConnectionString("DefaultConnection");

            //            services.AddDbContext<ApplicationDbContext>(opt => opt
            //#if DEBUG
            //                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            //#endif
            //                .UseSqlServer(cstr));
            //            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddHealthChecks().AddSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            services.AddHostedService<ProvisionerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map<HealthEndpoint>();
                endpoints.Map<InfoEndpoint>();
                //endpoints.MapControllers();
            });
        }
    }
}
