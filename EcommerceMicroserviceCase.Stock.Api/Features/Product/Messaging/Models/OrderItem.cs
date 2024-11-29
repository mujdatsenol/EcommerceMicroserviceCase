namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}