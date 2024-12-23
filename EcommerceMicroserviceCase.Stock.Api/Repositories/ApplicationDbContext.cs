using EcommerceMicroserviceCase.Stock.Api.Features.Product.Domain;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Stock.Api.Repositories;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}