using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IDockerService
    {
        Task<bool> CreateAndStartContainerAsync(string imageName);
        Task<bool> CreateAndStartContainerAsync(string imageName, List<string> envVariables);
        Task<bool> CreateAndStartContainerAsync(string imageName, List<string> envVariables, Dictionary<string, EmptyStruct> exposedPorts, HostConfig hostConfig);
        DockerClient CreateDockerClient();
        string GetDockerApiUri();
        Task PullImageAsync(string imageName);
    }
}