
using PublisherRabbitMQ;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri(""); //write AMQP URL

string messageRead;

do
{
    Console.Write("Please write your message for 50 times: ");
    messageRead = Console.ReadLine();
    if (messageRead != string.Empty && messageRead != null)
    {
        PublishMessage(messageRead);
    }
    else
    {
        Environment.Exit(0);
    }

} while (messageRead != null);

void PublishMessage(string message)
{
    Random randomEnumLogName = new Random();

    try
    {
        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("exchange-topic-logs", durable: true, type: ExchangeType.Topic);

            Enumerable.Range(0, 40).ToList().ForEach(x =>
            {
                LogNames.EnumLognames rndEnumLogname1 = (LogNames.EnumLognames)randomEnumLogName.Next(1, 5);
                LogNames.EnumLognames rndEnumLogname2 = (LogNames.EnumLognames)randomEnumLogName.Next(1, 5);
                LogNames.EnumLognames rndEnumLogname3 = (LogNames.EnumLognames)randomEnumLogName.Next(1, 5);

                var routeKey = $"{rndEnumLogname1}.{rndEnumLogname2}.{rndEnumLogname3}";

                message = $"{message}: {rndEnumLogname1}-{rndEnumLogname2}-{rndEnumLogname3}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("exchange-topic-logs", routeKey, null, messageBody);

                Console.WriteLine($"'{message}' is sended\n");
            });

            Console.WriteLine("All Logs are sended");
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.ToString()); }


}


