using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Dto;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging.Models;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Queries;
using EcommerceMicroserviceCase.Stock.Api.Repositories;
using MediatR;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Handlers;

public class GetProductsQuaryHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<GetProductsQuery, ServiceResult<List<ProductDto>>>
{
    public async Task<ServiceResult<List<ProductDto>>> Handle(
        GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository
            .GetAllAsync(cancellationToken: cancellationToken);
        
        var productsDto = mapper.Map<List<ProductDto>>(products);



        var test = new List<OrderItem>()
        {
            new() { ProductId = Guid.Parse("08dd1063-e9bb-3d6e-26b0-4d290a500000"), Quantity = 1 },
            new() { ProductId = Guid.Parse("08dd1065-f5d5-0bc8-26b0-4d290a500000"), Quantity = 1 },
            new() { ProductId = Guid.Parse("08dd1066-0101-7eb4-26b0-4d290a500000"), Quantity = 1 },
        };
        
        var ids = test.Select(s => s.ProductId).ToList();
        
        var qwde = await repository.GetByQueryAsync(q =>
            ids.Contains(q.Id), cancellationToken: cancellationToken);

        var asd = qwde.ToList();
        
        return ServiceResult<List<ProductDto>>.SuccessAsOk(productsDto);
    }
}