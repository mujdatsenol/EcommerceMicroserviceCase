using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;

public record IncrementMessageRetryCountCommand(Guid Id) : IRequestService;