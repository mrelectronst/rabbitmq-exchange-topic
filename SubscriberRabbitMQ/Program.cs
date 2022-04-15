
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri(""); //write AMQP URL

using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();

    channel.ExchangeDeclare("exchange-topic-logs", durable: true, type: ExchangeType.Topic);

    channel.BasicQos(0, 1, false);

    var subscriber = new EventingBasicConsumer(channel);

    string queueName = channel.QueueDeclare().QueueName;

    var routeKey = "*.Error.*";

    var routeKey1 = "Information.#";

    channel.QueueBind(queueName, "exchange-topic-logs", routeKey);

    channel.BasicConsume(queueName, false, subscriber);

    Console.WriteLine("Listening...");

    subscriber.Received += (object? sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(1500);

        Console.WriteLine($"Received Message : {message}");

        //File.AppendAllText("logs"+ queueName + ".txt", message + "\n");

        channel.BasicAck(e.DeliveryTag, false);
    };

    Console.ReadLine();
}