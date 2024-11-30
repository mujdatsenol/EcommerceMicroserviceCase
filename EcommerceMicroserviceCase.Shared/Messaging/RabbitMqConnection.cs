using RabbitMQ.Client;

namespace EcommerceMicroserviceCase.Shared.Messaging;

public class RabbitMqConnection(RabbitMqOption option) : IAsyncDisposable
{
    private readonly IConnectionFactory _connectionFactory =
        new ConnectionFactory
        {
            HostName = option.HostName,
            Port = option.Port,
            UserName = option.UserName,
            Password = option.Password
        };
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task StartAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public IChannel CreateChannel()
    {
        return _channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }
    }
}