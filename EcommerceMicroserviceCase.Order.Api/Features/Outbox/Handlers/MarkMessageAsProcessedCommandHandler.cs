using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Handlers;

public class MarkMessageAsProcessedCommandHandler(IRepository<OutboxMessage> repository)
    : IRequestHandler<MarkMessageAsProcessedCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(MarkMessageAsProcessedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await repository.GetByIdAsync(request.Id);
            if (message is null)
            {
                return ServiceResult.ErrorAsNotFound();
            }
        
            message.Processed = true;
            await repository.UpdateAsync(message, cancellationToken);
        
            Log.Information($"Outbox message received and processed.Id: {message.Id}");

            return ServiceResult.SuccessAsNoContent();
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}