using EcommerceMicroserviceCase.Order.Api.Repositories;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;

public class OutboxMessage : BaseEntity
{
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public bool Processed { get; set; }
    public int RetryCount { get; set; }
}