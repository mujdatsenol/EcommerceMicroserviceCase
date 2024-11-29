using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Queries;

public record GetOrderByIdQuary(Guid Id) : IRequestService<OrderDto>;