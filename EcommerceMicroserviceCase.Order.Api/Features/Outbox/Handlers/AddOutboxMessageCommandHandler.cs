using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MassTransit;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Handlers;

public class AddOutboxMessageCommandHandler(
    IRepository<OutboxMessage> repository, IMapper mapper)
    : IRequestHandler<AddOutboxMessageCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(
        AddOutboxMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = mapper.Map<OutboxMessage>(request);
            message.Id = NewId.NextSequentialGuid();
            message.CreatedAt = DateTimeOffset.UtcNow;
        
            await repository.AddAsync(message, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        
            Log.Information($"Outbox message added. Id: {message.Id}, EventType: {message.EventType}");
        
            return ServiceResult<Guid>.SuccessAsCreated(message.Id, message.Id.ToString());
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult<Guid>.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}