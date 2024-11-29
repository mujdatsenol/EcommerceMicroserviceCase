using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Filters;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;
using MediatR;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Endpoints;

public static class UpdateProductEndpoint
{
    public static RouteGroupBuilder UpdateProduct(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapPut("/",
                async (UpdateProductCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("UpdateProduct")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateProductCommand>>();
        
        return routeGroup;
    }
}