using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Quantity)
    : IRequestService;