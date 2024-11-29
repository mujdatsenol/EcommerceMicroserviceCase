namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;

public record CreateOrderItemCommand(
    Guid ProductId,
    string ProductName,
    string ProductDescription,
    int Quantity,
    decimal UnitPrice);