using k8s;
using k8s.Models;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IKubernetesService
    {
        Kubernetes GetKubernetesClient();
        V1PodList GetPods(string @namespace);
        V1Deployment GetEUConverterDeployment();
        V1Job GetEUConverterJob();
    }
}