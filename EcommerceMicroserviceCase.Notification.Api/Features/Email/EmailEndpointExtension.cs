using EcommerceMicroserviceCase.Notification.Api.Features.Email.Endpoints;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email;

public static class EmailEndpointExtension
{
    public static void MapEmailEndpoints(this WebApplication app)
    {
        app.MapGroup("api/emails")
            .WithName("Email")
            .GetEmails()
            .GetEmailById();
    }
}