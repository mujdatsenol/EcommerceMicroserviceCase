using AutoMapper;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Dto;
using EcommerceMicroserviceCase.Order.Api.Features.Orders.Queries;
using EcommerceMicroserviceCase.Order.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Handlers;

public class GetOrdersQuaryHandler(IRepository<Domain.Order> repository, IMapper mapper)
    : IRequestHandler<GetOrdersQuery, ServiceResult<List<OrderDto>>>
{
    public async Task<ServiceResult<List<OrderDto>>> Handle(
        GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(
            quary => quary.Include(e => e.OrderItems),
            cancellationToken);
        
        var productsDto = mapper.Map<List<OrderDto>>(products);
        
        return ServiceResult<List<OrderDto>>.SuccessAsOk(productsDto);
    }
}