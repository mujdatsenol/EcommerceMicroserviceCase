using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Repositories;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Handlers;

public class UpdateProductCommandHandler(IRepository<Domain.Product> repository)
    : IRequestHandler<UpdateProductCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (product is null)
            {
                return ServiceResult.ErrorAsNotFound();
            }
        
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
        
            await repository.UpdateAsync(product, cancellationToken);
            
            Log.Information($"Product updated. Id: {product.Id}");

            return ServiceResult.SuccessAsNoContent();
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult<Guid>.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}