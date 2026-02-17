using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
{
    private readonly IOptions<RabbitMqOptions> _options;
    private readonly ILogger<RabbitMqConnectionFactory> _logger;
    
    public RabbitMqConnectionFactory(
        ILogger<RabbitMqConnectionFactory> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _options = options;
    }
    
    public async Task<IConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Value.Host,
            Port = _options.Value.Port,
            UserName = _options.Value.Username,
            Password = _options.Value.Password
        };

        _logger.LogDebug("Creating RabbitMQ connection to {Host}:{Port}", _options.Value.Host, _options.Value.Port);
        return await factory.CreateConnectionAsync(ct);
    }

    public async Task<IChannel> CreateChannelAsync(CancellationToken ct = default)
    {
        var connection = await CreateConnectionAsync(ct);
        var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: "calculations",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: ct);

        return channel;
    }
}