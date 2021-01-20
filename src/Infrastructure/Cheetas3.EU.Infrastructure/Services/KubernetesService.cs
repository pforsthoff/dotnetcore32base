using k8s;
using Microsoft.Extensions.Logging;
using Cheetas3.EU.Application.Common.Interfaces;
using k8s.Models;
using System.Collections.Generic;

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
        public Kubernetes GetKubernetesClient()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile("assets/config");
            return new Kubernetes(config);
        }

        public V1PodList GetPods(string @namespace)
        {
            var list = _client.ListNamespacedPod(@namespace);
            return list;
        }
        public V1Deployment GetEUConverterDeployment()
        {
            V1Deployment deployment = new V1Deployment()
            {
                ApiVersion = "apps/v1",
                Kind = "Deployment",
                Metadata = new V1ObjectMeta()
                {
                    Name = "eu-converter-deployment",
                    NamespaceProperty = "default",
                    Labels = new Dictionary<string, string>()
                {
                    { "app", "eu-converter" }
                }
                },
                Spec = new V1DeploymentSpec
                {
                    Replicas = 1,
                    Selector = new V1LabelSelector()
                    {
                        MatchLabels = new Dictionary<string, string>
                    {
                        { "app", "eu-converter" }
                    }
                    },
                    Template = new V1PodTemplateSpec()
                    {
                        Metadata = new V1ObjectMeta()
                        {
                            CreationTimestamp = null,
                            Labels = new Dictionary<string, string>
                        {
                             { "app", "eu-converter" }
                        }
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>()
                        {
                            new V1Container()
                            {
                                Name = "eu-converter",
                                Image = "pguerette/euconverter",
                                ImagePullPolicy = "Always",
                                Ports = new List<V1ContainerPort> { new V1ContainerPort(80) }
                            }
                        }
                        }
                    }
                },
                Status = new V1DeploymentStatus()
                {
                    Replicas = 1
                }
            };
            return deployment;
        }
        public V1Job GetEUConverterJob()
        {
            V1Job job = new V1Job()
            {
                ApiVersion = "batch/v1",
                Kind = V1Job.KubeKind,
                Metadata = new V1ObjectMeta() { Name = "eu-converter-job" },
                Spec = new V1JobSpec()
                {
                    Template = new V1PodTemplateSpec()
                    {
                        Spec = new V1PodSpec()
                        {
                            Containers = new List<V1Container>()
                                {
                                    new V1Container()
                                    {
                                        Image = "pguerette/euconverter",
                                        Name = "eu-converter",
                                        Command = new List<string>() { "/bin/bash", "-c", "--" },
                                       
                                    },
                                },
                            RestartPolicy = "Never",
                        },
                    },
                }
            };
            return job;
        }
    }
}
