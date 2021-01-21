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

            services.AddSingleton<IConfigurationService, ConfigurationService>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddHealthActuator(Configuration);
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();
            services.AddInfoActuator(Configuration);
            services.AddSingleton<IInfoContributor, ConversionServiceInfoContributor>();

            //Sql Server Database Stuff
            var cstr = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddDbContext<ApplicationDbContext>(opt => opt
#if DEBUG
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
#endif
                .UseSqlServer(cstr),ServiceLifetime.Transient);
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddHealthChecks().AddSqlServer(cstr);

            var mqUri = new Uri(Configuration.GetConnectionString("RabbitMQ"));
            services.AddSingleton<IMessageQueueService, MessageQueueService>();
            services.AddHealthChecks().AddRabbitMQ(mqUri);

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
