using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcommerceMicroserviceCase.Shared.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Consumers;

public class CreateOrderConsumer(IServiceScopeFactory scopeFactory)
    : BackgroundService, IMessageConsumer
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _ = CosumeAsync("create-order-queue", "create-order-exchange", stoppingToken);
        }, stoppingToken);
    }

    public async Task CosumeAsync(string queueName, string exchangeName, CancellationToken cancellationToken)
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
            var messageBody = JsonSerializer.Deserialize<Order>(message,
                options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, });
            
            Console.WriteLine($"Message received: {message}");

            if (messageBody != null) await UpdateStock(messageBody);

            await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, cancellationToken);
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
            Console.WriteLine("Products quantity updated");
        }
        else
        {
            throw new Exception("Products quantity update failed");
        }
    }
}