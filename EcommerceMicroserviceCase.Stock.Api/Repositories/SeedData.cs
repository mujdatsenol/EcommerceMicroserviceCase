using EcommerceMicroserviceCase.StockService.Api.Features.Products.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.StockService.Api.Repositories;

public static class SeedData
{
    public static async Task AddSeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        await dbContext.Database.MigrateAsync();

        if (!dbContext.Products.Any())
        {
            var products = new List<Product>
            {
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Kazak",
                    Description = "Kazak açıklaması",
                    Price = 799.99m,
                    Quantity = 250,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Hırka",
                    Description = "Hırka açıklaması",
                    Price = 1299.99m,
                    Quantity = 150,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Pantolon",
                    Description = "Pantolon açıklaması",
                    Price = 950.50m,
                    Quantity = 300,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Ceket",
                    Description = "Ceket açıklaması",
                    Price = 2455.25m,
                    Quantity = 50,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Gömlek",
                    Description = "Gömlek açıklaması",
                    Price = 1350.29m,
                    Quantity = 400,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Kravat",
                    Description = "Kravat açıklaması",
                    Price = 385.35m,
                    Quantity = 480,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Çorap",
                    Description = "Çorap açıklaması",
                    Price = 85.75m,
                    Quantity = 500,
                    Created = DateTimeOffset.UtcNow
                },
            };
            
            dbContext.Products.AddRange(products);
            await dbContext.SaveChangesAsync();
        }
    }
}