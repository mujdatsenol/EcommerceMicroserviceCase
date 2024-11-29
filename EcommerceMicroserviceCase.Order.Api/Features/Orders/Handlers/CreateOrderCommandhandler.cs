using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;
using EcommerceMicroserviceCase.Order.Api.Helpers;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MassTransit;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Handlers;

public class CreateOrderCommandhandler(
    IRepository<Domain.Order> repository,
    IRepository<OrderItem> orderItemRepository,
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
        
        // RabbitMQ'ya Stock servisi için sipariş geldi stoktan düş mesajı verilecek.
        // RabbitMQ'ya Notification servisi için sipariş geldi mesaj gönder mesajı verilecek.
        
        return ServiceResult<CreateOrderResponse>.SuccessAsCreated(response, $"/api/orders/{response.Id}");
    }
}