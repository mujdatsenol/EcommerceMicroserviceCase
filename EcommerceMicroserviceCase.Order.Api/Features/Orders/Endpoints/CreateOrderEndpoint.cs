using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Filters;
using MediatR;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Endpoints;

public static class CreateOrderEndpoint
{
    public static RouteGroupBuilder CreateOrder(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapPost("/",
                async (CreateOrderCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateOrderCommand>>();
        
        return routeGroup;
    }
}