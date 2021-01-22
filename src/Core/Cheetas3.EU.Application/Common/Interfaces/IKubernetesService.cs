using k8s;
using k8s.Models;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IKubernetesService
    {
        Kubernetes GetKubernetesClient();
        V1PodList GetPods(string @namespace);
        V1Deployment GetEUConverterDeployment(string sliceId);
        V1Job GetEUConverterJob(int id);
        V1Job GetEUConverterAppSettingsSecret(int id);
        void CreateKubernetesSecretFromAppsettiings();
    }
}