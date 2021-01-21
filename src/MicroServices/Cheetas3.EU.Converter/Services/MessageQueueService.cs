using Cheetas3.EU.Converter.Extensions;
using Cheetas3.EU.Converter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Cheetas3.EU.Converter.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly ILogger<MessageQueueService> _logger;
        public IConfiguration Configuration { get; }
        readonly IModel _model;
        private static readonly string _exchangeName = "cheetas3.eu.exchange";
        private static readonly string _queueName = "cheetas3.eu.queue";
        private static readonly string _routingKey = "cheetas3.eu.routingkey";

        public MessageQueueService(IConfiguration configuration, ILogger<MessageQueueService> logger)
        {
            Configuration = configuration;
            _logger = logger;

            _logger.LogInformation("Configuring Message Queue Service");

            var rabbitMqCstr = Configuration.GetConnectionString("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMqCstr),
                AutomaticRecoveryEnabled = true
            };

            try
            {
                var connection = factory.CreateConnection();
                _model = connection.CreateModel();
                DeclareBindExchangeAndQueue();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create connection to RabbitMQ", ex);
            }
        }

        private void DeclareBindExchangeAndQueue()
        {
            _model.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            _model.QueueDeclare(_queueName, true, false, false, null);
            _model.QueueBind(_queueName, _exchangeName, _routingKey);
        }

        public void PublishMessage(Byte[] message)
        {
            if (_model != null)
                _model.BasicPublish(_exchangeName, _routingKey, null, message);
            else
                _logger.LogInformation($"Call to PublishMessage failed as there is no active connection to RabbitMQ");
        }
    }
}
