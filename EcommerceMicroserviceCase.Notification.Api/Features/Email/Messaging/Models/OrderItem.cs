namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging.Models;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string? ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}