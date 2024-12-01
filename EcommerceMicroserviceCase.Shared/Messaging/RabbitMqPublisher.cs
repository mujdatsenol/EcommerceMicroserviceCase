using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    
    public async Task PublishExchangeMessageAsync(
        string exchangeName,
        string routingKey,
        object message,
        string dlqExchange = "",
        string dlqRoutingKey = "dead_letter_queue")
    {
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        // Dead Letter Exchange (DLX)
        var  arguments = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", dlqExchange },
            { "x-dead-letter-routing-key", dlqRoutingKey }
        };
        
        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: arguments!);
        
        string serializeMessage = JsonSerializer.Serialize(message,
            options: new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
        
        var body = Encoding.UTF8.GetBytes(serializeMessage);
        
        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body,
            basicProperties: _props,
            mandatory: true);
        
        await Task.CompletedTask;
    }
    
    public async Task PublishQueueMessageAsync(
        string queueName,
        string message,
        string dlqExchange = "",
        string dlqRoutingKey = "dead_letter_queue")
    {
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        var  arguments = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", dlqExchange },
            { "x-dead-letter-routing-key", dlqRoutingKey }
        };
        
        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: arguments!);
        
        string serializeMessage = JsonSerializer.Serialize(message,
            options: new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
        
        var body = Encoding.UTF8.GetBytes(serializeMessage);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body,
            basicProperties: _props,
            mandatory: true);

        await Task.CompletedTask;
    }

    public async Task CreateDlqAsync(string dlqQueueName = "dead_letter_queue")
    {
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        await channel.QueueDeclareAsync(
            dlqQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null); 

        await Task.CompletedTask;
    }

    public async Task PublishMessageToDlqAsync(string dlqQueueName, object message)
    {
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        string serializeMessage = JsonSerializer.Serialize(message,
            options: new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
        
        var body = Encoding.UTF8.GetBytes(serializeMessage);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: dlqQueueName,
            body: body,
            basicProperties: _props,
            mandatory: true);
        
        await Task.CompletedTask;
    }
}