using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Order.Api.Repositories.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddDatabaseService(
        this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        bool isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        if (isDocker)
        {
            string? host = configuration["POSTGRES_DB_ORDER_HOST"];
            string? db = configuration["POSTGRES_DB_ORDER_NAME"];
            string? user = configuration["POSTGRES_DB_DEFAULT_USERNAME"];
            string? pass = configuration["POSTGRES_DB_DEFAULT_PASSWORD"];
            connectionString = $"Host={host};Port=5432;Database={db};Username={user};Password={pass};";
        }
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}