using EcommerceMicroserviceCase.Order.Api.Repositories;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Domain;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CustomerSurname { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public DateTimeOffset OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } = default!;
}