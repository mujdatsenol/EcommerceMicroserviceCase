using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;

public record AddOutboxMessageCommand(
    string EventType,
    string Payload)
    : IRequestService<Guid>;