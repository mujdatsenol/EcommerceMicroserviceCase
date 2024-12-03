using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcommerceMicroserviceCase.Shared.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Consumers;

public class CreateOrderConsumer(IServiceScopeFactory scopeFactory)
    : BackgroundService, IMessageConsumer
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _ = CosumeAsync("create-order-exchange", "stock-create-order-queue", stoppingToken);
            _ = ConsumeDlqAsync("dle-create-order-exchange", "stock-dlq-create-order-queue", stoppingToken);
        }, stoppingToken);
    }

    public async Task CosumeAsync(string exchangeName, string queueName, CancellationToken cancellationToken)
    {
        try
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
                var messageBody = JsonConvert.DeserializeObject<OutboxMessage>(message);
                if (messageBody != null)
                {
                    var order = JsonConvert.DeserializeObject<Order>(messageBody.Payload);
                    if (order != null)
                    {
                        Log.Information($"Stok servisi mesaj aldı. " +
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
        catch (Exception e)
        {
            Log.Error($"Message: {e.Message} | InnerException: {e.InnerException} | StackTrace: {e.StackTrace}");
            throw new Exception(e.Message, e.InnerException);
        }
    }

    public async Task ConsumeDlqAsync(string dlqExchangeName, string dlQueueName, CancellationToken cancellationToken)
    {
        try
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
                var messageBody = JsonConvert.DeserializeObject<OutboxMessage>(message);
                if (messageBody != null)
                {
                    var order = JsonConvert.DeserializeObject<Order>(messageBody.Payload);
                    if (order != null)
                    {
                        Log.Information($"Stok servisi DLQ'dan mesaj aldı. " +
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
        catch (Exception e)
        {
            Log.Error($"Message: {e.Message} | InnerException: {e.InnerException} | StackTrace: {e.StackTrace}");
            throw new Exception(e.Message, e.InnerException);
        }
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
            Log.Information("Ürünlerin stok bilgileri güncellendi.");
        }
        else
        {
            Log.Error("Products quantity update failed");
            throw new Exception("Products quantity update failed");
        }
    }
}