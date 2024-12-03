using EcommerceMicroserviceCase.Notification.Api.Repositories;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Domain;

public class Email : BaseEntity
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTimeOffset SendDate { get; set; }
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CustomerSurname { get; set; } = default!;
    public DateTimeOffset OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = default!;
}