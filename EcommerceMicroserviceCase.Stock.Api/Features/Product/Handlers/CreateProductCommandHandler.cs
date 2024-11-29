using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.StockService.Api.Repositories;
using MassTransit;
using MediatR;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Handlers;

public class CreateProductCommandHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<CreateProductCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var hasProduct = await repository
            .AnyAsync(a => a.Name == request.Name, cancellationToken);
        if (hasProduct)
        {
            return ServiceResult<Guid>.Error(
                "Product already exists", 
                $"Product name '{request.Name}' already exists", 
                HttpStatusCode.BadRequest);
        }
        
        var newProduct = mapper.Map<Domain.Product>(request);
        newProduct.Id = NewId.NextSequentialGuid();
        newProduct.Created = DateTimeOffset.UtcNow;
        
        await repository.AddAsync(newProduct, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        
        return ServiceResult<Guid>.SuccessAsCreated(newProduct.Id, $"/api/products/{newProduct.Id}");
    }
}