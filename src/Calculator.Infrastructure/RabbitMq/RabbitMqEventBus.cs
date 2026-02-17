using System.Text;
using System.Text.Json;
using Calculator.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;

namespace Calculator.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly IRabbitMqConnectionFactory _factory;
    private readonly string _queueName;

    public RabbitMqEventBus(
        ILogger<RabbitMqEventBus> logger,
        IRabbitMqConnectionFactory factory,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _factory = factory;
        _queueName = options.Value.QueueName;
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        await using var channel = await _factory.CreateChannelAsync();

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: _queueName,
            body: body);

        _logger.LogInformation("Event published: {@Event}", @event);
    }
}