using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Provisioner.UnitTests.Services
{
    public class DockerServiceTests
    {
        public readonly DockerClient _client;

        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public void CanCreateDockerClient()
        {
            var client = new DockerClientConfiguration().CreateClient();

            var platform = DockerApiUri();

            platform.Should().NotBeNull();
            client.Should().NotBeNull();
        }

        //Value based on current enviornment
        [TestCase("http://192.168.1.20:2375")]
        public void CanCreateDockerClientWithUri(string @namespace)
        {
            var uri = new Uri(@namespace);
            var client = new DockerClientConfiguration(uri).CreateClient();

            var platform = DockerApiUri();

            platform.Should().NotBeNull();
            client.Should().NotBeNull();
        }



        [Test]
        public async Task CanPullNewDockerImageAsync()
        {
            var imageName = "alpine:latest";

            DockerClient client = new DockerClientConfiguration().CreateClient();
            client.Should().NotBeNull();

            //Check to see if image exists
            //TODO: This seems to look at all images regardless of the MatchName paramter.
            IList<ImagesListResponse> images = await client.Images.ListImagesAsync(
            new ImagesListParameters()
            {
                MatchName = imageName,
            });

            //TODO: This doesn't seem to be impacted if the image doesn't exist to remove
            if (images.Any())
            {
                await client.Images.DeleteImageAsync(imageName,
                    new ImageDeleteParameters
                    {
                        Force = true
                    });
            }

            await client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = imageName
                }, null, new Progress<JSONMessage>());

            images = await client.Images.ListImagesAsync(
            new ImagesListParameters()
            {
                MatchName = imageName,
            });

            //This should be true"
            images.Should().Contain(r => r.RepoTags.Contains(imageName));
        }

        [Test]
        public async Task CanStartAContainerAsync()
        {
            var imageName = "nginx:latest";

            DockerClient client = new DockerClientConfiguration().CreateClient();
            client.Should().NotBeNull();


            //var images = (await client.Images.ListImagesAsync(new ImagesListParameters { All = true }))
            //    .Where(i => i.RepoTags.Any(t => t.Contains("alpine")))
            //    .Select(i => i.RepoTags.First());

            //IList<ImagesListResponse> images = await client.Images.ListImagesAsync(
            //    new ImagesListParameters()
            //    {
            //        All = true
            //    }) ;

            //var image = images.FirstOrDefault(p => p.RepoTags.Any(p => p.Contains(imageName)));
            //var imageId = image.ID.Substring(image.ID.IndexOf(":") + 1, 12);

            //Create The Container
            var result = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = imageName,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {
                        "8000", default
                    }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"8000", new List<PortBinding> {new PortBinding {HostPort = "8000" } }}
                    },
                    PublishAllPorts = true
                }
            });

            var containerId = result.ID;
            containerId.Should().NotBeNull();
            result.Should().NotBeNull();

            //Start The Container
            await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());

            //ref: https://docs.docker.com/engine/reference/commandline/ps/
            //Check to see if Container is running
            //var containersListParameters = new ContainersListParameters
            //{
            //    All = true,
            //    Filters = new Dictionary<string, IDictionary<string, bool>>()
            //    {
            //        ["ancestor"] = new Dictionary<string, bool>
            //        {
            //            [imageName] = true
            //        }
            //    },
            //};

            var containersListParameters = new ContainersListParameters
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>()
                {
                    ["id"] = new Dictionary<string, bool>
                    {
                        [containerId] = true
                    }
                },
            };

            var images = await client.Containers.ListContainersAsync(containersListParameters);
            images[0].State.Should().Be("running");


            //Kill the container
            await client.Containers.KillContainerAsync(containerId,
                new ContainerKillParameters(), CancellationToken.None );

            //Verify it's dead
            images = await client.Containers.ListContainersAsync(containersListParameters);
            images[0].State.Should().Be("exited");

        }

        [Test]
        public async Task CanFilterImagesAsync()
        {
            DockerClient client = new DockerClientConfiguration().CreateClient();
            client.Should().NotBeNull();

            var imageName = "alpine:latest";
            var imageListParameters = new ImagesListParameters
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>()
                {
                    ["before"] = new Dictionary<string, bool>
                    {
                        [imageName] = true
                    }
                },
            };
             var images = await client.Images.ListImagesAsync(imageListParameters);
            images.Should().NotBeNull();
          }

        [Test]
        public async Task CanGetAllClientDockerImagesAsync()
        {
            DockerClient client = new DockerClientConfiguration().CreateClient();

            IList<ImagesListResponse> images = await client.Images.ListImagesAsync(
            new ImagesListParameters()
            {
                All = true
            });

            images.Should().NotBeNull();
            images.Should().NotBeEmpty();
        }

        [Test]
        public async Task CanGetAllClientDockerContainersAsync()
        {
            DockerClient client = new DockerClientConfiguration().CreateClient();

            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
            new ContainersListParameters()
            {
                Limit = 10,
                All = true
            });

            containers.Should().NotBeNull();
            containers.Should().NotBeEmpty();
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
    }
}