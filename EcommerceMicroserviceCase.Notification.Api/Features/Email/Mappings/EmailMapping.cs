using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Mappings;

public class EmailMapping : Profile
{
    public EmailMapping()
    {
        CreateMap<CreateEmailCommand, Domain.Email>();
        CreateMap<CreateEmailOrderItemCommand, Domain.OrderItem>();
        CreateMap<Domain.Email, EmailDto>().ReverseMap();
        CreateMap<Domain.OrderItem, OrderItemDto>().ReverseMap();
    }
}