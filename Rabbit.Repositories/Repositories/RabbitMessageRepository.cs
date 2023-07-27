using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Rabbit.Repositories.Repositories
{
    public class RabbitMessageRepository : IRabbitMessageRepository
    {
        public void SendMessage(Message message)
        {
            var name = "queue";

            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "admin", Password = "admin", VirtualHost = "Teste" };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: name,
                                       durable: true,
                                     exclusive: false,
                                    autoDelete: false,
                                     arguments: null);

                    string json = JsonSerializer.Serialize(message);
                    byte[] body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "",
                                       routingKey: name,
                                  basicProperties: null,
                                             body: body);

                }
            }
        }
    }
}
