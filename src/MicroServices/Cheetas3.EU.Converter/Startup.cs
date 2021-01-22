using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Info;
using Cheetas3.EU.Converter.Actuators;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Cheetas3.EU.Converter.Services;
using Cheetas3.EU.Converter.Interfaces;
using Cheetas3.EU.Persistance;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;

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
            var cstrSql = Configuration.GetConnectionString("DefaultConnection");
            var cstrRmq = new Uri(Configuration.GetConnectionString("RabbitMQ"));

            //Application Services
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IMessageQueueService, MessageQueueService>();

            //DBContext
            services.AddDbContext<ApplicationDbContext>(opt => opt
#if DEBUG
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
#endif
                .UseSqlServer(cstrSql));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            //Custom Actuators
            services.AddHealthActuator(Configuration);
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();
            services.AddInfoActuator(Configuration);
            services.AddSingleton<IInfoContributor, ConversionServiceInfoContributor>();

            //Health Checks
            services.AddHealthChecks().AddCheck("RabbitMq Ping (100)", new PingHealthCheck("cheetasv3rbt", 100));
            services.AddHealthChecks().AddCheck("SQLServer Ping (100)", new PingHealthCheck("cheetasv3sql", 100));
            services.AddHealthChecks().AddRabbitMQ(cstrRmq);
            services.AddHealthChecks().AddSqlServer(cstrSql);



            //The Order of ConversionService should be last in the service collection
            services.AddHostedService<ConversionService>();
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
