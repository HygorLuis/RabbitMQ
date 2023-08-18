using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Rabbit.Repositories.Repositories
{
    public class RabbitMessageRepository : IRabbitMessageRepository
    {
        private IConnection _rabbitMQConnection;
        private IModel _rabbitMQChannel;
        private const string fila = "queue";

        public void SendMessage(Message message)
        {

            if (_rabbitMQConnection?.IsOpen != true)
            {
                CreateConnection();
                CreateQueue();
            }

            string json = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(json);

            var properties = _rabbitMQChannel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            _rabbitMQChannel.BasicPublish(exchange: "",
                               routingKey: fila,
                          basicProperties: properties,
                                     body: body);

        }

        #region Private

        public void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "Teste"
            };

            _rabbitMQConnection = factory.CreateConnection();
        }

        public void CreateQueue()
        {
            _rabbitMQChannel = _rabbitMQConnection.CreateModel();

            _rabbitMQChannel.QueueDeclare(queue: fila,
                                        durable: true,
                                      exclusive: false,
                                     autoDelete: false,
                                      arguments: null);
        }

        #endregion
    }
}
