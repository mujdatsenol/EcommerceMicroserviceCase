using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.StockService.Api.Repositories;
using MediatR;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Handlers;

public class UpdateProductCommandHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<UpdateProductCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (product is null)
        {
            return ServiceResult.ErrorAsNotFound();
        }
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        
        await repository.UpdateAsync(product, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}