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
        public V1Deployment GetEUConverterDeployment(string sliceId)
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
        public V1Job GetEUConverterJob(int id)
        {
            V1Job job = new V1Job()
            {
                ApiVersion = "batch/v1",
                Kind = V1Job.KubeKind,
                Metadata = new V1ObjectMeta() { Name = $"eu-converter-sliceid-{id}" },
                Spec = new V1JobSpec()
                {
                    TtlSecondsAfterFinished = 0,
                    Template = new V1PodTemplateSpec()
                    {
                        Spec = new V1PodSpec()
                        {
                            Containers = new List<V1Container>()
                            {
                                new V1Container()
                                {
                                    Image = "pguerette/euconverter:latest",
                                    Name = $"eu-converter-sliceid-{id}",
                                    //Command = new List<string>() { "/bin/bash", "-c", "--" },
                                    Env = new List<V1EnvVar>()
                                    {
                                        new V1EnvVar("SliceId", id.ToString()),
                                        new V1EnvVar("ServiceHealthEndPoint", "http://localhost:5000/actuator/health"),
                                        new V1EnvVar("SleepDuration", "60000")
                                    },
                                    VolumeMounts = new List<V1VolumeMount>()
                                    {
                                        new V1VolumeMount(
                                                mountPath: "/app/appsettings.json",
                                                name: "config-volume",
                                                subPath: "appsettings.json")
                                    },
                                    ImagePullPolicy = "Always"
                                },
                            },
                            Volumes = new List<V1Volume>()
                            {
                                new V1Volume()
                                {
                                    Name = "config-volume",
                                    ConfigMap = new V1ConfigMapVolumeSource()
                                    {
                                        Name = "app-config"
                                    }
                                }
                            },
                            RestartPolicy = "Never",
                        },
                    },
                }
            };
            return job;
        }

        public V1Deployment GetEUConverterDeployment()
        {
            throw new System.NotImplementedException();
        }
    }
}
