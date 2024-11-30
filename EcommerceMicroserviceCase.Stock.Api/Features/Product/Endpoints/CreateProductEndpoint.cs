using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Filters;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using MediatR;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Endpoints;

public static class CreateProductEndpoint
{
    public static RouteGroupBuilder CreateProduct(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapPost("/",
                async (CreateProductCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateProduct")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateProductCommand>>();
        
        return routeGroup;
    }
}