using EcommerceMicroserviceCase.Order.Api.Features.Orders.Endpoints;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders;

public static class OrderEndpointExtension
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGroup("api/orders")
            .WithName("Order")
            .CreateOrder()
            .GetOrders()
            .GetOrderById();
    }
}