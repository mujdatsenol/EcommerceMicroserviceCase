using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Repositories;
using MediatR;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Handlers;

public class UpdateProductsQuantityCommandHandler(IRepository<Domain.Product> repository)
    : IRequestHandler<UpdateProductsQuantityCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        UpdateProductsQuantityCommand request, CancellationToken cancellationToken)
    {
        var products = await repository
            .GetByQueryAsync(q => request.Products.ContainsKey(q.Id), cancellationToken: cancellationToken);

        foreach (var product in products)
        {
            product.Quantity -= request.Products[product.Id];
        }
        
        await repository.UpdateRangeAsync(products, cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }
}