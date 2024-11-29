using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Stock.Api.Repositories.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddDatabaseService(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}