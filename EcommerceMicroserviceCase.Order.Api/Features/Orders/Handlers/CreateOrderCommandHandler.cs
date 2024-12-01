using System.Text.Json;
using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Helpers;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Shared.Messaging;
using MassTransit;
using MediatR;

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
        
        // Outbox Message tablosuna diğer servislere gidecek mesaj için kayıt atılıyor.
        var outbox = await mediator.Send(
            new AddOutboxMessageCommand(
                "OrderCreated",
                JsonSerializer.Serialize(newOrder)),
            cancellationToken);
        
        // INFO: Outbox pattern uygulandı. Kullanılmıyor!
        // await CreateOrderMessage(newOrder, cancellationToken);
        
        return ServiceResult<CreateOrderResponse>.SuccessAsCreated(response, $"/api/orders/{response.Id}");
    }

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