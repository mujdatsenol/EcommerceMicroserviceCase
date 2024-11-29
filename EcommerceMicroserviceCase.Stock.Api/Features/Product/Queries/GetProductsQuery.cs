using EcommerceMicroserviceCase.Shared;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Dto;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Queries;

public record GetProductsQuery : IRequestService<List<ProductDto>>;