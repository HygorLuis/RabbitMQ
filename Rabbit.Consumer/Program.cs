using System.Text;
using System.Text.Json;
using Rabbit.Models.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var name = "queue";
var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "admin", Password = "admin", VirtualHost = "Teste" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: name,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj = JsonSerializer.Deserialize<Message>(message);
            Console.WriteLine($"Id: {obj.Id}\nMensagem: {obj.Mensagem}");
        };
        channel.BasicConsume(queue: name,
                             autoAck: true,
                             consumer: consumer);
    }
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
