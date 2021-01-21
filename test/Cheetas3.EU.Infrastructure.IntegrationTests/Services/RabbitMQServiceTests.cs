using NUnit.Framework;
using RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using RabbitMQ.Client;
using Cheetas3.EU.Application.Common.Extensions;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Cheetas3.EU.Infrastructure.IntegrationTests.Services
{
    //REF: https://www.rabbitmq.com/dotnet-api-guide.html
    public class RabbitMQServiceTests
    {
        IConnection _connection;
        IModel _model;
        private static string _exchangeName = "test_exchange";
        private static string _queueName = "test_queue";
        private static string _rountingKey = "test_key";
        private int _msgRcvdCount = 0;

        [OneTimeSetUp]
        public void Setup()
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "cheetas.local"
            };

            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += Consumer_Received;
            _model.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        [Test, Order(1)]
        public void CanConnectToRemoteBroker()
        {
            _connection.Should().NotBeNull();
            _model.Should().NotBeNull();
        }

        [Test, Order(2)]
        public void CanDeclareBindExchangeAndQueue()
        {
            bool testSuccess = true;

            try
            {
                _model.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
                _model.QueueDeclare(_queueName, true, false, false, null);
                _model.QueueBind(_queueName, _exchangeName, _rountingKey);
            }
            catch (Exception)
            {
                testSuccess = false;
            }

            _model.Should().NotBeNull();
            testSuccess.Should().BeTrue();
        }

        [Test, Order(3), Sequential]
        public void CanPublishMessage([Values("Message1", "Message2", "Message3", "Message4", "Message5")] string message)
        {
            bool testSuccess = true;
            try
            {
                _model.BasicPublish(_exchangeName, _rountingKey, null, message.ToByteArray());
            }
            catch (Exception)
            {
                testSuccess = false;
            }

            testSuccess.Should().BeTrue();

        }

        [Test, Order(4)]
        public void CanConsumeMessage()
        {
            var queueName = "provisioner_job_updates";
            var exchangeName = "test_exchange";

            _model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _model.QueueDeclare(queueName, true, false, false, null);
            _model.QueueBind(queueName, exchangeName, _rountingKey);

            _model.Should().NotBeNull();

            //var helper = new RabbitMQHelper(_model, "demo");
            //helper.SetupQueue("demoQueue");
            //helper.Should().NotBeNull();
        }





        [Test, Order(4)]
        public void CanReceiveMessage()
        {
            Thread.Sleep(1000); //Sleep the thread so the events have a chance to catch up.
            _msgRcvdCount.Should().BeGreaterThan(0);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = body.ToMessageString();
            _msgRcvdCount++;
        }
    }
}
