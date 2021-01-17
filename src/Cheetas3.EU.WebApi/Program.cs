using System;
using Microsoft.Extensions.Hosting;
using Cheetas3.EU.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Cheetas3.EU
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            Console.WriteLine("Starting Engineering Unit API");
            using (var scope = builder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    if (context.Database.IsSqlServer())
                        context.Database.Migrate();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            builder.Run();
        }

        private string DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                return "npipe://./pipe/docker_engine";
            }

            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isLinux)
            {
                return "unix:/var/run/docker.sock";
            }

            throw new Exception("Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
                //.UseUrls("http://*:5000")
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5000");
                    webBuilder.UseStartup<Startup>();
                });
            //host.ConfigureLogging((hostingContext, loggingBuilder) =>
            //{
            //    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //    loggingBuilder.AddDynamicConsole();
            //});
            return host;
        }
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    var builder = WebHost.CreateDefaultBuilder(args)
        //        .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
        //        .UseUrls("http://*:5000")
        //        .UseStartup<Startup>();
        //    builder.ConfigureLogging((hostingContext, loggingBuilder) =>
        //    {
        //        loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //        loggingBuilder.AddDynamicConsole();
        //    });
        //    return builder;
        //}
    }
}