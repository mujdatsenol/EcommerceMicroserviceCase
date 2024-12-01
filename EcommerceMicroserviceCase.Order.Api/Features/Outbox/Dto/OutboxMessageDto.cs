namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Dto;

public record OutboxMessageDto(
    Guid Id,
    string EventType,
    string Payload,
    DateTimeOffset CreatedAt,
    bool Processed,
    int RetryCount);