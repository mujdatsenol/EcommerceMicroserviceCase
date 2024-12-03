using EcommerceMicroserviceCase.Stock.Api.Repositories;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Domain;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset Created { get; set; }
}