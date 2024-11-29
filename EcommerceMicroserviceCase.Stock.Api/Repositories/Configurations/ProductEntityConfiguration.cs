using EcommerceMicroserviceCase.StockService.Api.Features.Product.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMicroserviceCase.StockService.Api.Repositories.Configurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Description)
            .HasMaxLength(200);
        
        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.Quantity)
            .IsRequired();
        
        builder.Property(x => x.Created)
            .IsRequired()
            .HasColumnType("timestamptz");
    }
}