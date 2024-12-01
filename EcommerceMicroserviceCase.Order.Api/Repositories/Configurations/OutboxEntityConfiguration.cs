using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceMicroserviceCase.Order.Api.Repositories.Configurations;

public class OutboxEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("Outbox")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(e => e.EventType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Payload)
            .IsRequired()
            .HasColumnType("jsonb");
        
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamptz");
        
        builder.Property(e => e.Processed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);
    }
}