using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Dto;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Queries;
using MediatR;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Endpoints;

public static class GetProductsEndpoint
{
    public static RouteGroupBuilder GetProducts(this RouteGroupBuilder group)
    {
        group.MapGet("/", 
                async (IMediator mediator) => 
                    (await mediator.Send(new GetProductsQuery())).ToGenericResult())
            .WithName("GetProducts")
            .Produces<List<ProductDto>>(StatusCodes.Status200OK);
        
        return group;
    }
}