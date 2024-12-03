using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Commands;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Domain;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox.Dto;

namespace EcommerceMicroserviceCase.Order.Api.Features.Outbox.Mappings;

public class OutboxMessageMapping : Profile
{
    public OutboxMessageMapping()
    {
        CreateMap<AddOutboxMessageCommand, OutboxMessage>();
        CreateMap<OutboxMessage, OutboxMessageDto>().ReverseMap();
    }
}