using EcommerceMicroserviceCase.Notification.Api.Features.Email.Domain;
using EcommerceMicroserviceCase.Shared;

namespace EcommerceMicroserviceCase.Notification.Api.Features.Email.Command;

public record CreateEmailCommand(
    string From,
    string To,
    string Subject,
    string Body,
    Guid OrderId,
    string OrderNumber,
    string CustomerName,
    string CustomerSurname,
    DateTimeOffset OrderDate,
    decimal TotalAmount,
    List<CreateEmailOrderItemCommand> OrderItems
    ) : IRequestService<Guid>;