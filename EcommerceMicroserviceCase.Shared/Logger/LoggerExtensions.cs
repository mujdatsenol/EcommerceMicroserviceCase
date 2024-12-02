using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.OpenSearch;

namespace EcommerceMicroserviceCase.Shared.Logger;

public static class LoggerExtensions
{
    public static IServiceCollection AddLogger(
        this IServiceCollection services, IConfiguration configuration)
    {
        bool isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        if (isDocker)
        {
            services.AddOptions<LoggerOption>()
                .Bind(configuration.GetSection("LoggerOption"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
        else
        {
            services.AddOptions<LoggerOption>()
                .BindConfiguration(nameof(LoggerOption))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
        
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<LoggerOption>>().Value);
        
        var loggerOptions = configuration.GetSection("LoggerOption").Get<LoggerOption>();
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(loggerOptions.OpenSearchUrl))
            {
                AutoRegisterTemplate = true,
                IndexFormat = loggerOptions.IndexFormat,
                FailureCallback = e => Console.WriteLine($"Log gönderilemedi: {e.Exception?.Message}"),
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog
            })
            .Enrich.FromLogContext()
            .CreateLogger();
        
        return services;
    }
    
    public static IServiceCollection UseLogger(this IServiceCollection services)
    {
        return services;
    }
}