using NUnit.Framework;
using RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using RabbitMQ.Client;

namespace Cheetas3.EU.Infrastructure.IntegrationTests.Services
{
    //REF: https://stackoverflow.com/questions/46962304/unit-test-rabbitmq-push-with-c-sharp-net-core
    public class RabbitMQServiceTests
    {
        IConnection _connection;
        IModel _model;
        private static string _exchangeName = "demo";
        const string ROUTING_KEY = "dummy_key";

        [OneTimeSetUp]
        public void Setup()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "cheetas.local";
            factory.VirtualHost = _exchangeName;

            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
        }

        [Test, Order(1)]
        public void CanConnectToRemoteBroker()
        {
            _connection.Should().NotBeNull();
            _model.Should().NotBeNull();
        }

        [Test, Order(2)]
        public void CanSetupMessageQueue()
        {
            var queueName = "demoQueue";

            _model.ExchangeDeclare(_exchangeName, ExchangeType.Topic);
            _model.QueueDeclare(queueName, true, false, false, null);
            _model.QueueBind(queueName, _exchangeName, ROUTING_KEY);

            _model.Should().NotBeNull();

            //var helper = new RabbitMQHelper(_model, "demo");
            //helper.SetupQueue("demoQueue");
            //helper.Should().NotBeNull();
        }

        [Test, Order(3)]
        public void CanPushMessageToQueue()
        {
            byte[] message = Encoding.ASCII.GetBytes("Test Message");
            _model.BasicPublish(_exchangeName, ROUTING_KEY, null, message);

        }
    }
}
