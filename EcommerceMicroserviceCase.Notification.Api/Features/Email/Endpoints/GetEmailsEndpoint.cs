using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Queries;
using EcommerceMicroserviceCase.Shared.Extensions;
using MediatR;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Endpoints;

public static class GetEmailsEndpoint
{
    public static RouteGroupBuilder GetEmails(this RouteGroupBuilder group)
    {
        group.MapGet("/", 
                async (IMediator mediator) => 
                    (await mediator.Send(new GetEmailsQuery())).ToGenericResult())
            .WithName("GetEmails")
            .Produces<List<EmailDto>>(StatusCodes.Status200OK);
        
        return group;
    }
}