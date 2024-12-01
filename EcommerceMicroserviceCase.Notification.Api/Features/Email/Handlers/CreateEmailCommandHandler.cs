using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;
using EcommerceMicroserviceCase.Notification.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MassTransit;
using MediatR;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Handlers;

public class CreateEmailCommandHandler(
    IRepository<Domain.Email> repository,
    IMapper mapper)
    : IRequestHandler<CreateEmailCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(
        CreateEmailCommand request, CancellationToken cancellationToken)
    {
        var mailId = NewId.NextSequentialGuid();
        var email = mapper.Map<Domain.Email>(request);
        
        email.Id = mailId;
        email.SendDate = DateTimeOffset.UtcNow;
        email.OrderItems.ForEach(f =>
        {
            f.Id = NewId.NextSequentialGuid();
            f.EmailId = mailId;
        });
        
        await repository.AddAsync(email, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        
        return ServiceResult<Guid>.SuccessAsCreated(mailId, $"/api/emails/{mailId}");
    }
}