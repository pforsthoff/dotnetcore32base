using k8s;
using k8s.Models;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IKubernetesService
    {
        V1PodList GetPods(string @namespace);
    }
}