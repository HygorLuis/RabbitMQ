/*********************** Reconhecimentos do consumidor ***********************
 * 
 * 
 * BasicAck (confirmações positivas): Confirmações positivas assumem que uma mensagem foi processada com sucesso, assim, instruem o RabbitMQ a registrar uma mensagem como entregue e pode ser descartada.
 * BasicNack (confirmações não positivas): Sugere que uma entrega não foi processada, porém a mensagem pode ser descartada, devolvidas ou recolocada na fila.
 * BasicReject (confirmações não positivas): Sugere que uma entrega não foi processada, mas ainda deve ser excluída. Possui uma limitação em comparação com BasicNack.
 * autoAck (confirmações automaticas): A mensagem é considerada entregue com sucesso imediatamente após o envio.
 * BasicQos: Define um limitador de mensagens para serem consumidas, evitando um alto fluxo e consumo de recursos da maquina
 * 
 * 
 * https://www.rabbitmq.com/confirms.html
 * https://www.rabbitmq.com/consumer-prefetch.html#single-consumer
*/

using System.Text;
using System.Text.Json;
using Rabbit.Models.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var name = "queue";
IConnection _rabbitMQConnection;
IModel _rabbitMQChannel;

CreateConnection();
CreateQueue();

var consumer = new EventingBasicConsumer(_rabbitMQChannel);
consumer.Received += (model, ea) =>
{
    try
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var obj = JsonSerializer.Deserialize<Message>(message);
        Console.WriteLine($"Id: {obj.Id}\nMensagem: {obj.Mensagem}");

        if (obj.Id == 0)
            throw new Exception("Teste Nack");

        _rabbitMQChannel.BasicAck(ea.DeliveryTag, false); // Processamento confirmado
    }
    catch (Exception ex)
    {
        _rabbitMQChannel.BasicNack(ea.DeliveryTag, false, true); // Processamento não confirmado, e reenfileira a mensagem novamente
    }
};

_rabbitMQChannel.BasicQos(0, 100, false);
_rabbitMQChannel.BasicConsume(queue: name,
                            autoAck: false,
                           consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

#region Private

void CreateConnection()
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

void CreateQueue()
{
    _rabbitMQChannel = _rabbitMQConnection.CreateModel();

    _rabbitMQChannel.QueueDeclare(queue: name,
                                durable: true,
                              exclusive: false,
                             autoDelete: false,
                              arguments: null);
}

#endregion