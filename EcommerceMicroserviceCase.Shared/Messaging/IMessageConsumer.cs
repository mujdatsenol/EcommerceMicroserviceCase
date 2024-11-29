namespace EcommerceMicroserviceCase.Shared.Messaging;

public interface IMessageConsumer
{
    Task CosumeAsync(
        string queueName,
        string exchangeName = default,
        CancellationToken cancellationToken = default);
}