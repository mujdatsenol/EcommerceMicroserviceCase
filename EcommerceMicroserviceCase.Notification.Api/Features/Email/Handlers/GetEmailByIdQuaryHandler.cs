using System.Net;
using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Dto;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Queries;
using EcommerceMicroserviceCase.Notification.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Handlers;

public class GetEmailByIdQuaryHandler(IRepository<Domain.Email> repository, IMapper mapper)
    : IRequestHandler<GetEmailByIdQuary, ServiceResult<EmailDto>>
{
    public async Task<ServiceResult<EmailDto>> Handle(
        GetEmailByIdQuary request, CancellationToken cancellationToken)
    {
        var email = await repository.GetByIdAsync(
            request.Id,
            query => query.Include(e => e.OrderItems),
            cancellationToken: cancellationToken);
        if (email is null)
        {
            return ServiceResult<EmailDto>.Error(
                "Email not found", 
                $"The email id {request.Id} was not found", 
                HttpStatusCode.NotFound);
        }
        
        var emailDto = mapper.Map<EmailDto>(email);
        
        return ServiceResult<EmailDto>.SuccessAsOk(emailDto);
    }
}