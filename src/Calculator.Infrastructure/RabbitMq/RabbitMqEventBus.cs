using System.Text;
using Calculator.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.RabbitMq;

namespace Calculator.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly IOptions<RabbitMqOptions> _options;

    public RabbitMqEventBus(
        ILogger<RabbitMqEventBus> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _options = options;
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var factory = new ConnectionFactory { HostName = _options.Value.Host, Port = _options.Value.Port, UserName = _options.Value.Username, Password = _options.Value.Password };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(queue: "calculations", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(@event?.ToString()!);
        
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "calculations", body: body);
        
        _logger.LogInformation("Event published: {@Event}", @event);
    }
}