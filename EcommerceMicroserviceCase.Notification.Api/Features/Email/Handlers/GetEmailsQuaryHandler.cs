using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Queries;
using EcommerceMicroserviceCase.Notification.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Handlers;

public class GetEmailsQuaryHandler(IRepository<Domain.Email> repository, IMapper mapper)
    : IRequestHandler<GetEmailsQuery, ServiceResult<List<EmailDto>>>
{
    public async Task<ServiceResult<List<EmailDto>>> Handle(
        GetEmailsQuery request, CancellationToken cancellationToken)
    {
        var emails = await repository.GetAllAsync(
            quary => quary.Include(e => e.OrderItems),
            cancellationToken);
        
        var emailsDto = mapper.Map<List<EmailDto>>(emails);
        
        return ServiceResult<List<EmailDto>>.SuccessAsOk(emailsDto);
    }
}