using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain = EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;

namespace EcommerceMicroserviceCase.Order.Api.Repositories.Configurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Domain.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Order> builder)
    {
        builder.ToTable("Order")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.CustomerName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.CustomerSurname)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.OrderDate)
            .IsRequired()
            .HasColumnType("timestamptz");
        
        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
    }
}