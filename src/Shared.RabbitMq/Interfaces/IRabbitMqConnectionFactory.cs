using RabbitMQ.Client;

namespace Shared.RabbitMq.Interfaces;

public interface IRabbitMqConnectionFactory
{
    Task<IConnection> CreateConnectionAsync(CancellationToken ct = default);
    Task<IChannel> CreateChannelAsync(CancellationToken ct = default);
}