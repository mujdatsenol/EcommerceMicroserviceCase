using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Dto;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Queries;
using EcommerceMicroserviceCase.StockService.Api.Repositories;
using MediatR;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Handlers;

public class GetProductsQuaryHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<GetProductsQuery, ServiceResult<List<ProductDto>>>
{
    public async Task<ServiceResult<List<ProductDto>>> Handle(
        GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        var productsDto = mapper.Map<List<ProductDto>>(products);
        return ServiceResult<List<ProductDto>>.SuccessAsOk(productsDto);
    }
}