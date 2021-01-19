using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using NUnit.Framework;

//Reference: https://www.danieldonbavand.com/dockerdotnet/

namespace Docker.DotNet.Integration.Tests.Setup
{
    public class DockerSetup
    {
        private DockerClient _dockerClient;

        private string _containerId;
        private const string ContainerImageUri = "amazon/dynamodb-local";


        [SetUp]
        public void Setup()
        {
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri())).CreateClient();
        }

        public async Task InitializeAsync()
        {
            await PullImage();
            await StartContainer();
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

            throw new Exception(
                "Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }

        [Test]
        private async Task PullImage()
        {
            await _dockerClient.Images
                .CreateImageAsync(new ImagesCreateParameters
                {
                    FromImage = ContainerImageUri,
                    Tag = "latest"
                },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }

        [Test]
        private async Task StartContainer()
        {
            var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = ContainerImageUri,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {
                        "8000", default(EmptyStruct)
                    }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"8000", new List<PortBinding> {new PortBinding {HostPort = "8000"}}}
                    },
                    PublishAllPorts = true
                }
            });

            _containerId = response.ID;

            await _dockerClient.Containers.StartContainerAsync(_containerId, null);
        }

        [Test]
        public async Task DisposeAsync()
        {
            if (_containerId != null)
            {
                await _dockerClient.Containers.KillContainerAsync(_containerId, new ContainerKillParameters());
            }
        }
    }
}