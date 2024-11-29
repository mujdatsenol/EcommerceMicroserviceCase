using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;

public record CreateOrderCommand(
    string CustomerName,
    string CustomerSurname,
    List<CreateOrderItemCommand> OrderItems)
    : IRequestService<CreateOrderResponse>;