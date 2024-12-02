using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcommerceMicroserviceCase.Shared.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Consumers;

public class CreateOrderConsumer(IServiceScopeFactory scopeFactory)
    : BackgroundService, IMessageConsumer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _ = CosumeAsync("create-order-exchange", "create-order-queue", stoppingToken);
            _ = ConsumeDlqAsync("dle-create-order-exchange", "dlq-create-order-queue", stoppingToken);
        }, stoppingToken);
    }

    public async Task CosumeAsync(string exchangeName, string queueName, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var rabbitMqConnection = scope.ServiceProvider.GetRequiredService<RabbitMqConnection>();
        
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
        
        await channel.QueueBindAsync(
            queueName,
            exchangeName,
            routingKey: string.Empty,
            cancellationToken: cancellationToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            var messageBody = JsonSerializer.Deserialize<OutboxMessage>(message, _jsonSerializerOptions);
            if (messageBody != null)
            {
                var order = JsonSerializer.Deserialize<Order>(messageBody.Payload, _jsonSerializerOptions);
                if (order != null)
                {
                    Log.Information($"Stock service received message. " +
                                    $"Exchange: {exchangeName} | Queue: {queueName} | Message: {message}");
                    await UpdateStock(order);
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
                }
                else
                {
                    Log.Error("Message body payload could not be deserialized.");
                }
            }
            else
            {
                Log.Error("Message could not be deserialized. Message body was null.");
            }
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, cancellationToken);
    }

    public async Task ConsumeDlqAsync(string dlqExchangeName, string dlQueueName, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var rabbitMqConnection = scope.ServiceProvider.GetRequiredService<RabbitMqConnection>();
        
        await rabbitMqConnection.StartAsync();
        var channel = rabbitMqConnection.CreateChannel();
        
        await channel.ExchangeDeclareAsync(
            dlqExchangeName,
            ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            dlQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
        
        await channel.QueueBindAsync(
            dlQueueName,
            dlqExchangeName,
            routingKey: string.Empty,
            cancellationToken: cancellationToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            var messageBody = JsonSerializer.Deserialize<OutboxMessage>(message, _jsonSerializerOptions);
            if (messageBody != null)
            {
                var order = JsonSerializer.Deserialize<Order>(messageBody.Payload, _jsonSerializerOptions);
                if (order != null)
                {
                    Log.Information($"Stock service received DLQ message. " +
                                    $"Exchange: {dlqExchangeName} | Queue: {dlQueueName} | Message: {message}");
                    await UpdateStock(order);
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
                }
                else
                {
                    Log.Error("DLQ Message body payload could not be deserialized.");
                }
            }
            else
            {
                Log.Error("DLQ Message could not be deserialized. Message body was null.");
            }
        };

        await channel.BasicConsumeAsync(dlQueueName, autoAck: false, consumer, cancellationToken);
    }

    private async Task UpdateStock(Order message)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        IDictionary<Guid, int> productsIds = message.OrderItems
            .ToDictionary(d => d.ProductId, d => d.Quantity);
        
        var requestResult = await mediator.Send(new UpdateProductsQuantityCommand(productsIds));
        if (requestResult.Success)
        {
            Log.Information("Products quantity updated");
        }
        else
        {
            Log.Error("Products quantity update failed");
            throw new Exception("Products quantity update failed");
        }
    }
}