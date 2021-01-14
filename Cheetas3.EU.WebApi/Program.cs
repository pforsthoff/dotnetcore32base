using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
//using Cheetas3.EUCommon;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging;

namespace Cheetas3.EU
{
    public class Program
    {
       // private readonly IRun processor;

        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args).Build();
            Console.WriteLine("Starting");

            builder.Run();
            //_ = new Program().Run();
        }

        //public Program(IRun processor, string[] args)
        //{
        //    this.processor = processor;
        //}

        //public Program()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine($"The time is now {DateTime.Now.ToShortTimeString()}");
        //        Thread.Sleep(60000);
        //    }
        //}

        //public Task Run()
        //{
        //    return Task.Run(() => processor.Run());
        //}

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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
                .UseUrls("http://*:8888")
                .UseStartup<Startup>();
            builder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddDynamicConsole();
            });
            return builder;
        }
    }
}