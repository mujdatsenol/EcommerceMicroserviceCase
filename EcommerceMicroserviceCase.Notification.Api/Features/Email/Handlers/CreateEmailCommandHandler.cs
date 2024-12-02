using AutoMapper;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;
using EcommerceMicroserviceCase.Notification.Api.Repositories;
using EcommerceMicroserviceCase.Shared;
using MassTransit;
using MediatR;
using Serilog;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Handlers;

public class CreateEmailCommandHandler(
    IRepository<Domain.Email> repository,
    IMapper mapper)
    : IRequestHandler<CreateEmailCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(
        CreateEmailCommand request, CancellationToken cancellationToken)
    {
        try
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
            
            Log.Information($"Email created. Id: {email.Id}");
        
            return ServiceResult<Guid>.SuccessAsCreated(mailId, $"/api/emails/{mailId}");
        }
        catch (Exception e)
        {
            Log.Error("Failed to email created");
            throw new Exception("Failed to email created");
        }
    }
}