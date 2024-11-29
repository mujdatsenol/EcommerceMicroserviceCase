namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Dto;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    DateTimeOffset Created);