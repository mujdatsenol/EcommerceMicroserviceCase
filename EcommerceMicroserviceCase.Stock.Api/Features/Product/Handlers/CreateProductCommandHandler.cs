using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Repositories;
using MassTransit;
using MediatR;
using Refit;
using Serilog;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Handlers;

public class CreateProductCommandHandler(IRepository<Domain.Product> repository, IMapper mapper)
    : IRequestHandler<CreateProductCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
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
            
            Log.Information($"Product created. Id: {newProduct.Id}");
        
            return ServiceResult<Guid>.SuccessAsCreated(newProduct.Id, $"/api/products/{newProduct.Id}");
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return ServiceResult<Guid>.Error(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}