using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Queries;
using EcommerceMicroserviceCase.Shared.Extensions;
using MediatR;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Endpoints;

public static class GetEmailByIdEndpoint
{
    public static RouteGroupBuilder GetEmailById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", 
                async (IMediator mediator, Guid id) => 
                    (await mediator.Send(new GetEmailByIdQuary(id))).ToGenericResult())
            .WithName("GetEmailById")
            .Produces<EmailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        
        return group;
    }
}