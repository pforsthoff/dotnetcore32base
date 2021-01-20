using k8s;
using k8s.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using System.Threading.Tasks;
using System.Threading;

namespace Cheetas3.EU.Infrastructure.IntegrationTests.Services
{
    class KubernetesServiceTests
    {

        private Kubernetes _client;
        private string _guidNS;
        private V1Namespace _namespace;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile("assets/config");
            _client = new Kubernetes(config);
            _guidNS = Guid.NewGuid().ToString();
            _namespace = new V1Namespace { Metadata = new V1ObjectMeta { Name = _guidNS } };
        }

        [TestCase("dev-admin@dev"), Order(1)]
        public void CheckConfigCurrentContext(string context)
        {
            var fi = new FileInfo("assets/config");
            var cfg = KubernetesClientConfiguration.BuildConfigFromConfigFile(fi, context, useRelativePaths: false);
            Assert.AreEqual(context, cfg.CurrentContext);
        }
        [Test, Order(2)]
        public async Task CanCreateNamespaceAsync()
        {
            var result = await _client.CreateNamespaceAsync(_namespace);
            result.Metadata.Name.Should().BeEquivalentTo(_guidNS);
        }

        [Test, Order(3)]
        public async Task CanDeleteNamespaceAsync()
        {
            var result = await _client.DeleteNamespaceAsync(_namespace.Metadata.Name, new V1DeleteOptions());
            result.HasObject.Should().BeTrue();
        }
    }
}
