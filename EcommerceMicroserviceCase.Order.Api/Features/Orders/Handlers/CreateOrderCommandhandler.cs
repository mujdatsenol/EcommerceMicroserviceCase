using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;
using EcommerceMicroserviceCase.Order.Api.Helpers;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Shared.Messaging;
using MassTransit;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Handlers;

public class CreateOrderCommandhandler(
    IRepository<Domain.Order> repository,
    IRepository<OrderItem> orderItemRepository,
    IMessagePublisher publisher,
    IMapper mapper)
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
        
        await repository.AddAsync(newOrder);
        await repository.SaveChangesAsync();
        
        var response = new CreateOrderResponse(newOrder.Id, newOrder.OrderNumber);
        
        // RabbitMQ'ya Exchange mesaj gönder. (Stock ve Notification servisleri abone olup kendi kuyruklarından dinlesinler)
        await CreateOrderMessage(newOrder, cancellationToken);
        
        return ServiceResult<CreateOrderResponse>.SuccessAsCreated(response, $"/api/orders/{response.Id}");
    }

    private async Task CreateOrderMessage(Domain.Order message, CancellationToken cancellationToken)
    {
        await publisher.PublishExchangeMessageAsync(
            "create-order-exchange",
            "create-order-queue",
            message);
    }
}