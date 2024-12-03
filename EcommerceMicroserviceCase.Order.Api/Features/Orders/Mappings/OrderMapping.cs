using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<CreateOrderCommand, Domain.Order>();
        CreateMap<CreateOrderItemCommand, OrderItem>();
        CreateMap<Domain.Order, OrderDto>()
            .ConstructUsing(x => new OrderDto(
                x.Id,
                x.OrderNumber,
                x.CustomerName,
                x.CustomerSurname,
                x.CustomerEmail,
                x.OrderDate,
                x.TotalAmount,
                x.OrderItems.Select(s => new OrderItemDto(
                    s.Id,
                    s.OrderId,
                    s.ProductId,
                    s.ProductName,
                    s.ProductDescription,
                    s.Quantity,
                    s.UnitPrice,
                    s.Subtotal)).ToList()));
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}