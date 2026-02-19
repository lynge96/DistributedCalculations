using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.RabbitMq.Interfaces;

namespace Shared.RabbitMq;

public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory, IAsyncDisposable
{
    private readonly IOptions<RabbitMqOptions> _options;
    private readonly ILogger<RabbitMqConnectionFactory> _logger;
    
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private IConnection? _connection;
    
    public RabbitMqConnectionFactory(
        ILogger<RabbitMqConnectionFactory> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _options = options;
    }
    
    public async Task<IConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        if (_connection is { IsOpen: true })
            return _connection;

        await _connectionLock.WaitAsync(ct);
        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            var factory = new ConnectionFactory
            {
                HostName = _options.Value.Host,
                Port = _options.Value.Port,
                UserName = _options.Value.Username,
                Password = _options.Value.Password
            };

            _logger.LogInformation("Creating RabbitMQ connection to {Host}:{Port}", factory.HostName, factory.Port);

            const int maxRetries = 5;
            var retryDelay = 1000;

            for (var attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _connection = await factory.CreateConnectionAsync(ct);
                    _logger.LogInformation("RabbitMQ connection established on attempt {Attempt}", attempt);
                    return _connection;
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    _logger.LogWarning(ex, "Failed to connect to RabbitMQ (attempt {Attempt}/{MaxRetries}). Retrying in {Delay}ms...", attempt, maxRetries, retryDelay);
                    await Task.Delay(retryDelay, ct);
                    retryDelay *= 2;
                }
            }
            
            _connection = await factory.CreateConnectionAsync(ct);
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task<IChannel> CreateChannelAsync(CancellationToken ct = default)
    {
        var connection = await CreateConnectionAsync(ct);
        var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: _options.Value.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: ct);

        return channel;
    }
    
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_connection is not null)
                await _connection.CloseAsync();
        }
        finally
        {
            _connection?.Dispose();
            _connection = null;
            _connectionLock.Dispose();
        }
    }
}