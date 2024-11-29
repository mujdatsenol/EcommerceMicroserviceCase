using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    int Quantity)
    : IRequestService<Guid>;