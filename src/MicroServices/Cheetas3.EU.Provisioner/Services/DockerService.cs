using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cheetas3.EU.Provisioner.Services
{
    public class DockerService
    {
        public readonly DockerClient _client;
        public DockerService(string dockerUri)
        {
            _client =  new DockerClientConfiguration(new Uri(dockerUri)).CreateClient();
        }

        public async Task<List<ContainerListResponse>> ListContainersAsync()
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(
            new ContainersListParameters()
            {
                Limit = 10,
            });

            return (List<ContainerListResponse>)containers;
        }

        public async Task PullImageAsync(string image)
        {

            var progress = new Progress<JSONMessage>();

            var parameters = new ImagesCreateParameters();
            parameters.FromImage = image;
            parameters.Tag = "latest";
            parameters.FromSrc = string.Empty;
            parameters.Repo = string.Empty;

            var authConfig = new AuthConfig();
            authConfig.Email = string.Empty;
            authConfig.Username = string.Empty;
            authConfig.Password = string.Empty;

            await _client.Images.CreateImageAsync(parameters, authConfig, progress);


        }

        public async Task<bool> StartContainerAsync(string image)
        {
            ContainerStartParameters p = new ContainerStartParameters();
            return await _client.Containers.StartContainerAsync(image, p);
        }
    }
}
