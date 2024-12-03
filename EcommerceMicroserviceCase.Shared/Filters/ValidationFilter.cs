using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceMicroserviceCase.Shared.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            return await next(context);
        }

        var firstParameter = context.Arguments.OfType<T>().FirstOrDefault();
        if (firstParameter is null)
        {
            return await next(context);
        }
        
        var validationResult = await validator.ValidateAsync(firstParameter);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        
        return await next(context);
    }
}

// Endpoint ana grubun da Common kullanım için alternatif
public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator>();
        if (validator is null)
        {
            return await next(context);
        }

        for (int i = 0; i < context.Arguments.Count; i++)
        {
            var argument = context.Arguments[i];
            if (argument is null)
            {
                return await next(context);
            }

            if (argument.GetType() != typeof(ValidationContext<>))
            {
                return await next(context);
            }
            
            var contextGenericType = typeof(ValidationContext<>).MakeGenericType(argument.GetType());
            var validationContext = Activator.CreateInstance(contextGenericType, argument) as IValidationContext;
            var results = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            if (!results.IsValid)
            {
                return Results.ValidationProblem(results.ToDictionary());
            }
        }
        
        return await next(context);
    }
}