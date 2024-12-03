using EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Consumers;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessageConsumers(this IServiceCollection services)
    {
        services.AddHostedService<CreateOrderConsumer>();
        return services;
    }
}