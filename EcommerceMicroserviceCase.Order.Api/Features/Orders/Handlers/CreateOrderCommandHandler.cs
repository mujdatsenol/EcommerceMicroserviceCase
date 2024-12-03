using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Helpers;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Shared.Messaging;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Handlers;

public class CreateOrderCommandHandler(
    IRepository<Domain.Order> repository,
    IMessagePublisher publisher,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<CreateOrderCommand, ServiceResult<CreateOrderResponse>>
{
    public async Task<ServiceResult<CreateOrderResponse>> Handle(
        CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guid orderId = NewId.NextSequentialGuid();
            var newOrder = mapper.Map<Domain.Order>(request);

            newOrder.Id = orderId;
            newOrder.OrderNumber = OrderHelper.GenerateOrderNumber();
            newOrder.OrderDate = DateTimeOffset.UtcNow;
            newOrder.OrderItems.ForEach(f =>
            {
                f.Id = NewId.NextSequentialGuid();
                f.OrderId = orderId;
                f.Subtotal = f.UnitPrice * f.Quantity;
            });
            newOrder.TotalAmount = newOrder.OrderItems.Sum(x => x.Subtotal);
        
            await repository.AddAsync(newOrder, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        
            var response = new CreateOrderResponse(newOrder.Id, newOrder.OrderNumber);
        
            Log.Information($"Order created. Id: {newOrder.Id}");
        
            // Outbox Message tablosuna diğer servislere gidecek mesaj için kayıt atılıyor.
            var outboxMessage = JsonConvert.SerializeObject(newOrder);
            var outbox = await mediator.Send(
                new AddOutboxMessageCommand("OrderCreated",outboxMessage), cancellationToken);
            if (outbox.IsFail)
            {
                string errorMessage = $"Outbox message could not be created for Order Created. Order Id: {newOrder.Id}";
                Log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
        
            // INFO: Outbox pattern uygulandı. Kullanılmıyor!
            // await CreateOrderMessage(newOrder, cancellationToken);
        
            return ServiceResult<CreateOrderResponse>.SuccessAsCreated(response, $"/api/orders/{response.Id}");
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult<CreateOrderResponse>.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }

    // Outbox Message sistemi olmadığı durum da örnek olması için bırakıldı.
    [Obsolete]
    private async Task CreateOrderMessage(Domain.Order message)
    {
        // Önce Dead Letter Queue yoksa oluştur
        await publisher.CreateDlqAsync("dlq-create-order-queue");
        
        await publisher.PublishExchangeMessageAsync(
            "create-order-exchange",
            "create-order-queue",
            message,
            dlqExchange: "dle-create-order-exchange",
            dlqRoutingKey: "dlq-create-order-queue");
    }
}