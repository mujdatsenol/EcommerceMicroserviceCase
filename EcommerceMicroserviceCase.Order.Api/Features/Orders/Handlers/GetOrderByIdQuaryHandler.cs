using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Queries;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Handlers;

public class GetOrderByIdQuaryHandler(IRepository<Domain.Order> repository, IMapper mapper)
    : IRequestHandler<GetOrderByIdQuary, ServiceResult<OrderDto>>
{
    public async Task<ServiceResult<OrderDto>> Handle(
        GetOrderByIdQuary request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(
            request.Id,
            query => query.Include(e => e.OrderItems),
            cancellationToken: cancellationToken);
        if (order is null)
        {
            return ServiceResult<OrderDto>.Error(
                "Order not found", 
                $"The order id {request.Id} was not found", 
                HttpStatusCode.NotFound);
        }
        
        var orderDto = mapper.Map<OrderDto>(order);
        
        return ServiceResult<OrderDto>.SuccessAsOk(orderDto);
    }
}