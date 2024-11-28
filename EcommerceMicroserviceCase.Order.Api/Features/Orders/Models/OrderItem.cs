using EcommerceMicroserviceCase.Order.Api.Repositories;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Models;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string? ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }

    public Order Order { get; set; } = default!;
}