using Cheetas3.EU.Actuators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Info;
using Microsoft.AspNetCore.Mvc;
using Cheetas3.EU.Persistance;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Application;
using Cheetas3.EU.Infrastructure;
using Cheetas3.EU.Services;
using Microsoft.AspNetCore.Http;

namespace Cheetas3.EU
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cstr = Configuration.GetConnectionString("DefaultConnection");

            services.AddHealthActuator(Configuration);
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();
            services.AddInfoActuator(Configuration);
            services.AddSingleton<IInfoContributor, ArbitraryInfoContributor>();

            services.AddHealthChecks().AddSqlServer(cstr);

            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "EU Provisioner API";
                //configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                //{
                //    Type = OpenApiSecuritySchemeType.ApiKey,
                //    Name = "Authorization",
                //    In = OpenApiSecurityApiKeyLocation.Header,
                //    Description = "Type into the textbox: Bearer {your JWT token}."
                //});

                //configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Using SteelToe for Health/Info
            //app.UseHealthChecks("/health");
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map<HealthEndpoint>();
                endpoints.Map<InfoEndpoint>();
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Nothing to see here!");
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });
        }
    }
}