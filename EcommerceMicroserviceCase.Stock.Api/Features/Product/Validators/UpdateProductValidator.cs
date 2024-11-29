using EcommerceMicroserviceCase.StockService.Api.Features.Product.Commands;
using FluentValidation;

namespace EcommerceMicroserviceCase.StockService.Api.Features.Product.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(3, 100).WithMessage("{{PropertyName} must be between 3 and 100 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("{PropertyName} must bu max 200 characters");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}