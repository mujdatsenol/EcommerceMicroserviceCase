using System.Net;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Handlers;

public class IncrementMessageRetryCountCommandHandler(IRepository<OutboxMessage> repository)
    : IRequestHandler<IncrementMessageRetryCountCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        IncrementMessageRetryCountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await repository.GetByIdAsync(request.Id);
            if (message is null)
            {
                return ServiceResult.ErrorAsNotFound();
            }
        
            message.RetryCount++;
            await repository.UpdateAsync(message, cancellationToken);
        
            Log.Information($"Outbox message not received. Retry count increased.. Id: {message.Id}, RetryCount: {message.RetryCount}");

            return ServiceResult.SuccessAsNoContent();
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}