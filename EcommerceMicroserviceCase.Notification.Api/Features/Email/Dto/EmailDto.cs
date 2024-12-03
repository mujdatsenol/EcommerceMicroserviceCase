namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;

public record EmailDto(
    Guid Id,
    string From,
    string To,
    string Subject,
    string Body,
    DateTimeOffset SentDate,
    Guid OrderId,
    string OrderNumber,
    string CustomerName,
    string CustomerSurname,
    DateTimeOffset OrderDate,
    decimal TotalAmount,
    IEnumerable<OrderItemDto> OrderItems);