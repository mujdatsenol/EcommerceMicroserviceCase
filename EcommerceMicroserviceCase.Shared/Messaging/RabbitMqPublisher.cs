using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace EcommerceMicroserviceCase.Shared.Messaging;

public class RabbitMqPublisher(RabbitMqConnection rabbitMqConnection) : IMessagePublisher
{
    private readonly BasicProperties _props = new BasicProperties
    {
        Persistent = true,
        ContentEncoding = "UTF-8",
        ContentType = "application/json"
    };
    
    public async Task PublishExchangeMessageAsync(string exchangeName, string routingKey, object message)
    {
        var channel = rabbitMqConnection.CreateChannel();
        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null);
        
        string serializeMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializeMessage);
        
        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body,
            basicProperties: _props,
            mandatory: true);
        
        await Task.CompletedTask;
    }
    
    public async Task PublishQueueMessageAsync(string queueName, string message)
    {
        var channel = rabbitMqConnection.CreateChannel();
        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var body = Encoding.UTF8.GetBytes(message);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body,
            basicProperties: _props,
            mandatory: true);

        await Task.CompletedTask;
    }
}