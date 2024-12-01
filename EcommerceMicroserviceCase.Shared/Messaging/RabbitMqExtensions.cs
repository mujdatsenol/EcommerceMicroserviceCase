using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EcommerceMicroserviceCase.Shared.Messaging;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMqService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        bool isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        if (isDocker)
        {
            services.AddOptions<RabbitMqOption>()
                .Bind(configuration.GetSection("RabbitMq"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
        else
        {
            services.AddOptions<RabbitMqOption>()
                .BindConfiguration(nameof(RabbitMqOption))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
        
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMqOption>>().Value);
        
        services.AddSingleton(sp =>
        {
            var option = sp.GetRequiredService<RabbitMqOption>();
            return new RabbitMqConnection(option);
        });
        
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        return services;
    }
}