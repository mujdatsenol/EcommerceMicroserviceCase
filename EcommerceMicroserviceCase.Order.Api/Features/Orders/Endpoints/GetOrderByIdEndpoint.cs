using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Queries;
using EcommerceMicroserviceCase.Shared.Extensions;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Endpoints;

public static class GetOrderByIdEndpoint
{
    public static RouteGroupBuilder GetOrderById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", 
                async (IMediator mediator, Guid id) => 
                    (await mediator.Send(new GetOrderByIdQuary(id))).ToGenericResult())
            .WithName("GetOrderById")
            .Produces<OrderDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        
        return group;
    }
}