using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Dto;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Queries;
using EcommerceMicroserviceCase.StockService.Api.Repositories;
using MediatR;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Handlers;

public class GetProductByIdQuaryHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ServiceResult<ProductDto>>
{
    public async Task<ServiceResult<ProductDto>> Handle(
        GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (product is null)
        {
            return ServiceResult<ProductDto>.Error(
                "Product not found", 
                $"The product id {request.Id} was not found", 
                HttpStatusCode.NotFound);
        }
        
        var productDto = mapper.Map<ProductDto>(product);
        
        return ServiceResult<ProductDto>.SuccessAsOk(productDto);
    }
}