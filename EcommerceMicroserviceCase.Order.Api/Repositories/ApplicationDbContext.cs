using Microsoft.EntityFrameworkCore;
using Models = EcommerceMicroserviceCase.Order.Api.Features.Orders.Models;

namespace EcommerceMicroserviceCase.Order.Api.Repositories;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Models.Order> Orders { get; set; }
    public DbSet<Models.OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}