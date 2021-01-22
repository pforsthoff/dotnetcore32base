using Cheetas3.EU.Application.Common.Events;
using Cheetas3.EU.Application.Common.Extensions;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

namespace Cheetas3.EU.Infrastructure.Services
{

    public class MessageQueueService : IMessageQueueService
    {
        private readonly ILogger<MessageQueueService> _logger;
        public IConfiguration Configuration { get; }
        public IApplicationDbContext _context;
        readonly IModel _model;
        private static readonly string _exchangeName = "cheetas3.eu.exchange";
        private static readonly string _queueName = "cheetas3.eu.queue";
        private static readonly string _routingKey = "cheetas3.eu.routingkey";

        //public event EventHandler MessageReceived;

        public MessageQueueService(IConfiguration configuration,
                                   IApplicationDbContext context,
                                   ILogger<MessageQueueService> logger)
        {
            Configuration = configuration;
            _logger = logger;
            _context = context;
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
                var consumer = new EventingBasicConsumer(_model);
                consumer.Received += Consumer_Received;
                _model.BasicConsume(queue: _queueName,
                                    autoAck: true,
                                    consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create connection to RabbitMQ", ex);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var array = e.Body.ToArray();
            var json = array.ToMessageString();
            Slice slice = json.FromJson(typeof(Slice)) as Slice;
        }

        private void DeclareBindExchangeAndQueue()
        {
            _model.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            _model.QueueDeclare(_queueName, true, false, false, null);
            _model.QueueBind(_queueName, _exchangeName, _routingKey);
        }

        //private void Consumer_Received(object sender, SliceEventArgs e)
        //{
        //    OnMessageReceived(e);
        //}

        public void PublishMessage(string message)
        {
            if (_model != null)
                _model.BasicPublish(_exchangeName, _routingKey, null, message.ToByteArray());
            else
                _logger.LogInformation($"Call to PublishMessage failed as there is no active connection to RabbitMQ");
        }

        public void ConsumeMessage()
        {
            var t = 1;
        }

        //protected virtual void OnMessageReceived(SliceEventArgs e)
        //{
        //    EventHandler handler = MessageReceived;
        //    handler?.Invoke(this, e);
        //}
    }
}
