using Domain = EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMicroserviceCase.Order.Api.Repositories.Configurations;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<Domain.OrderItem>
{
    public void Configure(EntityTypeBuilder<Domain.OrderItem> builder)
    {
        builder.ToTable("OrderItem")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.ProductId)
            .IsRequired();
        
        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.ProductDescription)
            .HasMaxLength(200);
        
        builder.Property(x => x.Quantity)
            .IsRequired();
        
        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.Subtotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);
    }
}