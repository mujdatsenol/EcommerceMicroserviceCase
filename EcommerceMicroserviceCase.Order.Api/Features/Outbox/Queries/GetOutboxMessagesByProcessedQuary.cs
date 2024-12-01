using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Dto;
using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Queries;

public record GetOutboxMessagesByProcessedQuary(string EventType) : IRequestService<List<OutboxMessageDto>>;