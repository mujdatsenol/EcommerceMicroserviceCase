using AutoMapper;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Commands;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Dto;

namespace EcommerceMicroserviceCase.Stock.Api.Features.Product.Mappings;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<CreateProductCommand, Domain.Product>();
        CreateMap<Domain.Product, ProductDto>().ReverseMap();
    }
}