namespace EcommerceMicroserviceCase.Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishExchangeMessageAsync(string exchangeName, string routingKey, object message);
    Task PublishQueueMessageAsync(string queueName, string message);
}