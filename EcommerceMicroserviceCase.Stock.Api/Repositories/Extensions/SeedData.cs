using EcommerceMicroserviceCase.Stock.Api.Features.Product.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Stock.Api.Repositories.Extensions;

public static class SeedData
{
    public static async Task AddSeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        if (!dbContext.Products.Any())
        {
            var products = new List<Product>
            {
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-5f51-0000-000000000000"),
                    Name = "Kazak",
                    Description = "Kazak açıklaması",
                    Price = 799.99m,
                    Quantity = 250,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a1e-0000-000000000000"),
                    Name = "Hırka",
                    Description = "Hırka açıklaması",
                    Price = 1299.99m,
                    Quantity = 150,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a2b-0000-000000000000"),
                    Name = "Pantolon",
                    Description = "Pantolon açıklaması",
                    Price = 950.50m,
                    Quantity = 300,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a2c-0000-000000000000"),
                    Name = "Ceket",
                    Description = "Ceket açıklaması",
                    Price = 2455.25m,
                    Quantity = 50,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a2e-0000-000000000000"),
                    Name = "Gömlek",
                    Description = "Gömlek açıklaması",
                    Price = 1350.29m,
                    Quantity = 400,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a40-0000-000000000000"),
                    Name = "Kravat",
                    Description = "Kravat açıklaması",
                    Price = 385.35m,
                    Quantity = 480,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1301-6e1d-6a41-0000-000000000000"),
                    Name = "Çorap",
                    Description = "Çorap açıklaması",
                    Price = 85.75m,
                    Quantity = 500,
                    Created = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("08dd1311-d9e5-19eb-0000-000000000000"),
                    Name = "Test Ürün",
                    Description = "Test ürün açıklaması",
                    Price = 125.75m,
                    Quantity = 78,
                    Created = DateTimeOffset.UtcNow
                }
            };
            
            dbContext.Products.AddRange(products);
            await dbContext.SaveChangesAsync();
        }
    }
}