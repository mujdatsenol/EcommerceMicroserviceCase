using EcommerceMicroserviceCase.Stock.Api.Features.Product.Endpoints;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product;

public static class ProductEndpointExtension
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGroup("api/products")
            .WithName("Product")
            .CreateProduct()
            .UpdateProduct()
            .GetProducts()
            .GetProductById();
        
        //.AddEndpointFilter<ValidationFilter>();
    }
}