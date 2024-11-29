using EcommerceMicroserviceCase.StockService.Api.Features.Product.Endpoints;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product;

public static class ProductEndpointExtension
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGroup("api/products")
            .WithName("Product")
            .CreateProduct();
        
        //.AddEndpointFilter<ValidationFilter>();
    }
}