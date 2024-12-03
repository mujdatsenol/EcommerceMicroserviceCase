using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;

public record UpdateProductsQuantityCommand(IDictionary<Guid, int> Products) : IRequestService;