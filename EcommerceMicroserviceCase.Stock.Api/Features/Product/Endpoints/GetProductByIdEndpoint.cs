using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Dto;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Queries;
using MediatR;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Endpoints;

public static class GetProductByIdEndpoint
{
    public static RouteGroupBuilder GetProductById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", 
                async (IMediator mediator, Guid id) => 
                    (await mediator.Send(new GetProductByIdQuery(id))).ToGenericResult())
            .WithName("GetProductById")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        
        return group;
    }
}