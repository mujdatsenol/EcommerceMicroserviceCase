using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;

public record MarkMessageAsProcessedCommand(Guid Id) : IRequestService;