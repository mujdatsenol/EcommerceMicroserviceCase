using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Queries;

public record GetEmailsQuery() : IRequestService<List<EmailDto>>;