using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetCore32Base.Data.Models;
using DotNetCore32Base.Data.Repositories;
using DotNetCore32Base.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetCore32Base
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RepositoryPatternDemoContext>(options => options.UseSqlServer(Configuration["Database:ConnectionString"]));
            //services.AddDbContext<RepositoryPatternDemoContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddControllersWithViews();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    var message = $"Host: {Environment.MachineName}\n" +
            //        $"EnvironmentName: {env.EnvironmentName}\n" +
            //        $"Secret value: {Configuration["Database:ConnectionString"]}";
            //    await context.Response.WriteAsync(message);
            //});
        }
    }
}