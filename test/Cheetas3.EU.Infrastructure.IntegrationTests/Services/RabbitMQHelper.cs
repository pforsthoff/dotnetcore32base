using RabbitMQ.Client;

namespace Cheetas3.EU.Infrastructure.IntegrationTests.Services
{
    public class RabbitMQHelper
    {
        private static IModel _model;
        private static string _exchangeName;
        const string RoutingKey = "dummy-key.";

        public RabbitMQHelper(IModel model, string exchangeName)
        {
            _model = model;
            _exchangeName = exchangeName;
        }

        public void SetupQueue(string queueName)
        {
            _model.ExchangeDeclare(_exchangeName, ExchangeType.Topic);
            _model.QueueDeclare(queueName, true, false, false, null);
            _model.QueueBind(queueName, _exchangeName, RoutingKey);
        }

        public void PushMessageIntoQueue(byte[] message, string queue)
        {
            SetupQueue(queue);
            _model.BasicPublish(_exchangeName, RoutingKey, null, message);
        }

        public byte[] ReadMessageFromQueue(string queueName)
        {
            SetupQueue(queueName);
            byte[] message;
            var data = _model.BasicGet(queueName, false);
            message = data.Body.ToArray();
            _model.BasicAck(data.DeliveryTag, false);
            return message;
        }
    }
}
