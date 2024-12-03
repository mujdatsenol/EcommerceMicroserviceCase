using EcommerceMicroserviceCase.Shared.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Consumers;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessageConsumers(this IServiceCollection services)
    {
        services.AddHostedService<CreateOrderConsumer>();
        return services;
    }
}