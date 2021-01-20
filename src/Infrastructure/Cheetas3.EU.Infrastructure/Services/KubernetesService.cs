using k8s;
using Microsoft.Extensions.Logging;
using Cheetas3.EU.Application.Common.Interfaces;
using System.IO;
using k8s.Models;

namespace Cheetas3.EU.Infrastructure.Services
{
    public class KubernetesService : IKubernetesService
    {
        private readonly ILogger<KubernetesService> _logger;
        private readonly IKubernetes _client;
        public KubernetesService (ILogger<KubernetesService> logger)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile("assets/config");
            _client = new Kubernetes(config);
        }

        public V1PodList GetPods(string @namespace)
        {
            var list = _client.ListNamespacedPod(@namespace);
            return list;
        }
    }
}
