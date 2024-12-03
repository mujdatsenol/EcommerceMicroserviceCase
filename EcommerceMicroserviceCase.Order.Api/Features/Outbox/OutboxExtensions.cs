namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox;

public static class OutboxExtensions
{
    public static IServiceCollection AddOutboxMessaging(this IServiceCollection services)
    {
        services.AddHostedService(provider =>
        {
            var scope = provider.GetService<IServiceScopeFactory>();
            if (scope != null)
                return new OutboxMessageProcessor(
                    scopeFactory: scope,
                    eventType: "OrderCreated",
                    exchangeName: "create-order-exchange",
                    routingKey: String.Empty, 
                    dlqExchange: "dle-create-order-exchange",
                    dlqRoutingKey: "dlq-create-order-queue");
            throw new InvalidOperationException($"The outbox messaging has not been configured.");
        });
        return services;
    }
}