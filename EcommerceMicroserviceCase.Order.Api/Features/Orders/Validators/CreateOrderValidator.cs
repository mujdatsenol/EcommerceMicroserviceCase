using EcommerceMicroserviceCase.Order.Api.Features.Orders.Commands;
using FluentValidation;

namespace EcommerceMicroserviceCase.Order.Api.Features.Orders.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(2, 50).WithMessage("{{PropertyName} must be between 3 and 100 characters");
        
        RuleFor(x => x.CustomerSurname)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(2, 50).WithMessage("{{PropertyName} must be between 3 and 100 characters");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty. Must contain at least one item.")
            .Must(items => items != null && items.Any()).WithMessage("The item list cannot be empty.");

        RuleForEach(x => x.OrderItems)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("{PropertyName} is required");

                item.RuleFor(x => x.ProductName)
                    .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                    .Length(3, 100).WithMessage("{{PropertyName} must be between 3 and 100 characters");

                item.RuleFor(x => x.ProductDescription)
                    .MaximumLength(200).WithMessage("{PropertyName} must bu max 200 characters");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
            });
    }
}