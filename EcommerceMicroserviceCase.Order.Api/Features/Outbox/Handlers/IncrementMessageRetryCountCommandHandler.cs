using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Handlers;

public class IncrementMessageRetryCountCommandHandler(IRepository<OutboxMessage> repository)
    : IRequestHandler<IncrementMessageRetryCountCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        IncrementMessageRetryCountCommand request, CancellationToken cancellationToken)
    {
        var message = await repository.GetByIdAsync(request.Id);
        if (message is null)
        {
            return ServiceResult.ErrorAsNotFound();
        }
        
        message.RetryCount++;
        await repository.UpdateAsync(message, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}