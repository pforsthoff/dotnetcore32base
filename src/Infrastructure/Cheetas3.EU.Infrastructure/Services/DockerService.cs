using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cheetas3.EU.Application.Common.Interfaces;


namespace Cheetas3.EU.Infrastructure.Services
{
    public class DockerService : IDockerService
    {
        private readonly ILogger<DockerService> _logger;
        private readonly DockerClient _dockerClient;

        public DockerService(ILogger<DockerService> logger, string dockerServiceUrl = "localhost")
        {
            _logger = logger;
            if(dockerServiceUrl == "localhost")
                _dockerClient = CreateDockerClient();
            else
                _dockerClient = CreateDockerClient(dockerServiceUrl);
        }

        public DockerClient CreateDockerClient(string url)
        {
            var uri = new Uri(url);
            return new DockerClientConfiguration(uri).CreateClient();
        }
        public DockerClient CreateDockerClient()
        {
            return new DockerClientConfiguration().CreateClient();
        }

        public async Task PullImageAsync(string imageName)
        {
            await _dockerClient.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = imageName
                },
                null, //Configure AuthConfig property if authorization is required to pull image
                new Progress<JSONMessage>());
        }


        public async Task<bool> CreateAndStartContainerAsync(string imageName)
        {
            //Create The Container
            var container = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = imageName
            });

            //StartTheContainer
            return await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
        }

        public async Task<bool> CreateAndStartContainerAsync(string imageName, List<string> envVariables)
        {

            var startPosition = imageName.LastIndexOf('/') + 1;
            var endPosition = imageName.IndexOf(':');
            var length = endPosition - startPosition;

            var containerName =  $"{imageName.Substring(startPosition, length) }_{envVariables[2].Replace('=','_').ToLower()}";

            //Create The Container
            var container = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = imageName,
                Env = envVariables,
                Name = containerName
            });

            //StartTheContainer
            return await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
        }

        public async Task<bool> CreateAndStartContainerAsync(string imageName,
            List<string> envVariables,
            Dictionary<string, EmptyStruct> exposedPorts,
            HostConfig hostConfig)
        {
            //Create The Container
            var container = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Env = envVariables,
                Image = imageName,
                ExposedPorts = exposedPorts,
                //ExposedPorts = new Dictionary<string, EmptyStruct>
                //{
                //    {
                //        "8000", default
                //    }
                //},
                HostConfig = hostConfig
                //HostConfig = new HostConfig
                //{
                //    PortBindings = new Dictionary<string, IList<PortBinding>>
                //    {
                //        {"8000", new List<PortBinding> {new PortBinding {HostPort = "8000" } }}
                //    },
                //    PublishAllPorts = true
                //}
            });

            //StartTheContainer
            return await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
        }

        public string GetDockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                _logger.LogInformation("Docker running on Windows OS");
                return "npipe://./pipe/docker_engine";
            }

            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isLinux)
            {
                _logger.LogInformation("Docker running on Linux OS");
                return "unix:/var/run/docker.sock";
            }

            _logger.LogError("Unable to determine what OS is running");
            throw new Exception("Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }

    }
}
