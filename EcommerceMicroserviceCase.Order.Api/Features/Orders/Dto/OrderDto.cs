namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;

public record OrderDto(
    Guid Id,
    string OrderNumber,
    string CustomerName,
    string CustomerSurname,
    DateTimeOffset OrderDate,
    decimal TotalPrice,
    List<OrderItemDto> OrderItems);