using Microsoft.Extensions.DependencyInjection;

namespace EcommerceMicroserviceCase.Shared.Extensions;

public static class ServerExtension
{
    public static IServiceCollection ConfigureCors(
        this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        
        return service;
    }
}