using AutoMapper;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.StockService.Api.Features.Product.Dto;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Mappings;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<CreateProductCommand, Domain.Product>();
        CreateMap<Domain.Product, ProductDto>().ReverseMap();
    }
}