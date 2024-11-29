using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EcommerceMicroserviceCase.Shared.Messaging;

public class RabbitMqConsumer(RabbitMqConnection rabbitMqConnection) : IMessageConsumer
{
    public async Task CosumeAsync(string queueName, string exchangeName, CancellationToken cancellationToken)
    {
        var channel = rabbitMqConnection.CreateChannel();
        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            Console.WriteLine($"Message received: {message}");

            // Mesajı işleme kodu buraya gelecek

            await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, cancellationToken);
    }
}