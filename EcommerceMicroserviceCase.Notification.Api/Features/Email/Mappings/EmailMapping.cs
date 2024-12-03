using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Models;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Mappings;

public class EmailMapping : Profile
{
    public EmailMapping()
    {
        CreateMap<CreateEmailCommand, Domain.Email>();
        CreateMap<CreateEmailOrderItemCommand, Domain.OrderItem>();
        CreateMap<OrderItem, CreateEmailOrderItemCommand>();
        CreateMap<Domain.Email, EmailDto>()
            .ConstructUsing(x => new EmailDto(
                x.Id,
                x.From,
                x.To,
                x.Subject,
                x.Body,
                x.SendDate,
                x.OrderId,
                x.OrderNumber,
                x.CustomerName,
                x.CustomerName,
                x.OrderDate,
                x.TotalAmount,
                x.OrderItems.Select(s => new OrderItemDto(
                    s.Id,
                    s.EmailId,
                    s.ProductId,
                    s.ProductName,
                    s.ProductDescription,
                    s.Quantity,
                    s.UnitPrice,
                    s.Subtotal
                ))
            ));
        CreateMap<Domain.OrderItem, OrderItemDto>().ReverseMap();
    }
}