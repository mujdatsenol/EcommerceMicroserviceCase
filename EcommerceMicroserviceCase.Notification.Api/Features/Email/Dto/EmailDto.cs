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
    DateTime OrderDate,
    decimal TotalAmount,
    List<OrderItemDto> OrderItems);