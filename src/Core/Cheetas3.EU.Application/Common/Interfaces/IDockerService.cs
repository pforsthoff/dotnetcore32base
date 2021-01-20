using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IDockerService
    {
        void CreateDockerClient();
        void CreateDockerClient(string url);
        Task<bool> CreateAndStartContainerAsync(string imageName);
        Task<bool> CreateAndStartContainerAsync(string imageName, List<string> envVariables);
        Task<bool> CreateAndStartContainerAsync(string imageName, List<string> envVariables, Dictionary<string, EmptyStruct> exposedPorts, HostConfig hostConfig);
        string GetDockerApiUri();
        Task PullImageAsync(string imageName);
    }
}