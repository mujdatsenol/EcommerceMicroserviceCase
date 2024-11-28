using EcommerceMicroserviceCase.StockService.Api.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.StockService.Api.Repositories;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}