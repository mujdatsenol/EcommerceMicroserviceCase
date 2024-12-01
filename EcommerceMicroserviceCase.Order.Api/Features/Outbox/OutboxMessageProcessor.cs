using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Queries;
using EcommerceMicroserviceCase.Shared.Messaging;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox;

public class OutboxMessageProcessor(
    IServiceScopeFactory scopeFactory,
    string eventType,
    string exchangeName,
    string routingKey,
    string dlqExchange,
    string dlqRoutingKey) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

            var outboxMessages = await mediator
                .Send(new GetOutboxMessagesByProcessedQuary(eventType), stoppingToken);

            foreach (var message in outboxMessages.Data!)
            {
                try
                {
                    // Önce Dead Letter Queue yoksa oluştur
                    await publisher.CreateDlqAsync(dlqRoutingKey);
                    
                    // Mesajı diğer servisler için RabbitMQ'ya gönder DLQ işaretli şekilde.
                    await publisher.PublishExchangeMessageAsync(
                        exchangeName,
                        routingKey,
                        message,
                        dlqExchange,
                        dlqRoutingKey);
                    
                    await mediator.Send(new MarkMessageAsProcessedCommand(message.Id), stoppingToken);
                }
                catch (Exception e)
                {
                    await mediator.Send(new IncrementMessageRetryCountCommand(message.Id), stoppingToken);

                    if (message.RetryCount >= 5)
                    {
                        Console.WriteLine("The message is forwarded to Dead Letter Queue...");
                        // 5. kez hata almışsa DLQ'ya manuel gönderme işlemi.
                        await publisher.PublishMessageToDlqAsync(dlqRoutingKey, message);
                    }
                }
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}