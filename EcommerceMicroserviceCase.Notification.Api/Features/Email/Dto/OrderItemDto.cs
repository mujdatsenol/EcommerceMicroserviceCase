namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;

public record OrderItemDto(
    Guid Id,
    Guid MailId,
    Guid ProductId,
    string ProductName,
    string? ProductDescription,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal);