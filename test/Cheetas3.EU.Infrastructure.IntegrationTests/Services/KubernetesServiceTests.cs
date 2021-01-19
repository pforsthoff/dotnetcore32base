using k8s;
using k8s.Models;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cheetas3.EU.Infrastructure.IntegrationTests.Services
{
    class KubernetesServiceTests
    {
        [TestCase("apps-admin@apps")]
        public void CheckConfigCurrentContext(string context)
        {
            var fi = new FileInfo("assets/config");
            var cfg = KubernetesClientConfiguration.BuildConfigFromConfigFile(fi, context, useRelativePaths: false);
            Assert.AreEqual(context, cfg.CurrentContext);
        }
        [TestCase("nunit-test")]
        public void CreateReadNamespace(string @namespace)
        {
            // Load from the default kubeconfig on the machine.
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile("assets/config");

            // Use the config object to create a client.
            var client = new Kubernetes(config);

            var ns = new V1Namespace
            {
                Metadata = new V1ObjectMeta
                {
                    Name = @namespace
                }
            };
            try
            {
                //delete namespace if exists
                var status = client.DeleteNamespace(ns.Metadata.Name, new V1DeleteOptions());
            }
            catch (System.Exception)
            {
            }
            ns = new V1Namespace
            {
                Metadata = new V1ObjectMeta
                {
                    Name = @namespace
                }
            };
            var result = client.CreateNamespace(ns);
            var namespaces = client.ListNamespace();
            foreach (var name in namespaces.Items)
            {
                if(name.Metadata.Name == @namespace)
                {
                    Assert.AreEqual(name.Metadata.Name, @namespace);
                }
            }
        }
    }
}
