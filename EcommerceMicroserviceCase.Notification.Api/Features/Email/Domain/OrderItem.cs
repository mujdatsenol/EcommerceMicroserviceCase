using EcommerceMicroserviceCase.Notification.Api.Repositories;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Domain;

public class OrderItem : BaseEntity
{
    public Guid EmailId { get; set; }
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string? ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }

    public Email Email { get; set; } = default!;
}