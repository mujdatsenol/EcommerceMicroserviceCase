using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Dto;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Queries;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Handlers;

public class GetOutboxMessagesByProcessedQuaryHandler(
    IRepository<OutboxMessage> repository, IMapper mapper)
    : IRequestHandler<GetOutboxMessagesByProcessedQuary, ServiceResult<List<OutboxMessageDto>>>
{
    public async Task<ServiceResult<List<OutboxMessageDto>>> Handle(
        GetOutboxMessagesByProcessedQuary request, CancellationToken cancellationToken)
    {
        var messages = await repository
            .GetByQueryAsync(q =>
                q.EventType == request.EventType
                && !q.Processed
                && q.RetryCount < 5, cancellationToken: cancellationToken);
        
        var messageDto = messages is null
            ? new()
            : mapper.Map<List<OutboxMessageDto>>(messages.ToList());
        
        return ServiceResult<List<OutboxMessageDto>>.SuccessAsOk(messageDto);
    }
}