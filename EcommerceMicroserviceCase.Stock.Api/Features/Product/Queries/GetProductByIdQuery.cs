using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Dto;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Queries;

public record GetProductByIdQuery(Guid Id) : IRequestService<ProductDto>;