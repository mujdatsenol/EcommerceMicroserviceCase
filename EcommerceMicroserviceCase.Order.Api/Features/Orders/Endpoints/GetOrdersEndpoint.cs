using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Queries;
using EcommerceMicroserviceCase.Shared.Extensions;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Endpoints;

public static class GetOrdersEndpoint
{
    public static RouteGroupBuilder GetOrders(this RouteGroupBuilder group)
    {
        group.MapGet("/", 
                async (IMediator mediator) => 
                    (await mediator.Send(new GetOrdersQuery())).ToGenericResult())
            .WithName("GetOrders")
            .Produces<List<OrderDto>>(StatusCodes.Status200OK);
        
        return group;
    }
}