using System.Net;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Repositories;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Handlers;

public class UpdateProductsQuantityCommandHandler(IRepository<Domain.Product> repository)
    : IRequestHandler<UpdateProductsQuantityCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        UpdateProductsQuantityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await repository
                .GetByQueryAsync(q => request.Products.ContainsKey(q.Id), cancellationToken: cancellationToken);

            foreach (var product in products)
            {
                product.Quantity -= request.Products[product.Id];
                Log.Information($"Product quantity updated. Id: {product.Id} - Quantity: {product.Quantity}");
            }
        
            await repository.UpdateRangeAsync(products, cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult<Guid>.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}