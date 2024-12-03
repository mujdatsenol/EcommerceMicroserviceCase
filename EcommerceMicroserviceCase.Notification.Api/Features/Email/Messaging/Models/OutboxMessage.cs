namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Models;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public bool Processed { get; set; }
    public int RetryCount { get; set; }
}