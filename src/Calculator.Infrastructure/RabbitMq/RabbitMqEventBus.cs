using Calculator.Application.Interfaces;
using RabbitMQ.Client;

namespace Calculator.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        throw new NotImplementedException();
    }
}