namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CustomerSurname { get; set; } = default!;
    public DateTimeOffset OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}