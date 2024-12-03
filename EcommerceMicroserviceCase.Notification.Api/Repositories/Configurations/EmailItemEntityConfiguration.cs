using EcommerceMicroserviceCase.Notification.Api.Features.Email.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMicroserviceCase.Notification.Api.Repositories.Configurations;

public class EmailItemEntityConfiguration  : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("Email")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.From)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.To)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Body)
            .IsRequired();
        
        builder.Property(x => x.SendDate)
            .IsRequired()
            .HasColumnType("timestamptz");
        
        builder.Property(x => x.OrderId)
            .IsRequired();
        
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
            .WithOne(x => x.Email)
            .HasForeignKey(x => x.EmailId);
    }
}