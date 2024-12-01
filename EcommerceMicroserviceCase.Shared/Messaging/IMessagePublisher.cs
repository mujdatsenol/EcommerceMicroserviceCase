namespace EcommerceMicroserviceCase.Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishExchangeMessageAsync(
        string exchangeName,
        string routingKey,
        object message,
        string dlqExchange = "",
        string dlqRoutingKey = "dead_letter_queue");
    
    Task PublishQueueMessageAsync(
        string queueName,
        string message,
        string dlqExchange = "",
        string dlqRoutingKey = "dead_letter_queue");
    
    Task CreateDlqAsync(string dlqQueueName = "dead_letter_queue");

    Task PublishMessageToDlqAsync(string dlqQueueName, object message);
}