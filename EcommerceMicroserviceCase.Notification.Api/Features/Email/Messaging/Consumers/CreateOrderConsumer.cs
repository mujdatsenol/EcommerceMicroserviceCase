using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Models;
using EcommerceMicroserviceCase.Shared.Messaging;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Consumers;

public class CreateOrderConsumer(IServiceScopeFactory scopeFactory)
    : BackgroundService, IMessageConsumer
{
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
            var messageBody = JsonSerializer.Deserialize<Order>(message,
                options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, });
            
            Console.WriteLine($"Message received: {message}");

            if (messageBody != null) await SendMail(messageBody);

            await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
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
            var messageBody = JsonSerializer.Deserialize<Order>(message,
                options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, });
            
            Console.WriteLine($"DLQ Message received: {message}");

            if (messageBody != null) await SendMail(messageBody);

            await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
        };

        await channel.BasicConsumeAsync(dlQueueName, autoAck: false, consumer, cancellationToken);
    }
    
    private async Task SendMail(Order message)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var orderItems = mapper.Map<List<CreateEmailOrderItemCommand>>(message.OrderItems);
        var createEmailRequest = new CreateEmailCommand(
            "ecommerce@ecommerce.com",
            message.CustomerEmail,
            "Order submitted",
            "Email body...",
            message.Id,
            message.OrderNumber,
            message.CustomerName,
            message.CustomerSurname,
            message.OrderDate,
            message.TotalAmount,
            orderItems);
        
        var requestResult = await mediator.Send(createEmailRequest);
        if (requestResult.Success)
        {
            Console.WriteLine("Sending email...");
            Console.WriteLine($"Email sent to customer: {message.CustomerName} {message.CustomerSurname} - {message.CustomerEmail}");
            Console.WriteLine($"For email details, please see here: {requestResult.UrlAsCreated}");
        }
        else
        {
            throw new Exception("Failed to sent email");
        }
    }
}