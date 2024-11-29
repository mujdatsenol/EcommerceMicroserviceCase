namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;

public record OrderItemDto(
    Guid Id,
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    string? ProductDescription,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal);