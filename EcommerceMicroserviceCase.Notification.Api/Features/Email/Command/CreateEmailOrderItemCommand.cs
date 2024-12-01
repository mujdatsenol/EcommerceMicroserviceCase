namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;

public record CreateEmailOrderItemCommand(
    Guid ProductId,
    string ProductName,
    string? ProductDescription,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal);