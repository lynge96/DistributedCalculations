using Calculator.Application.Interfaces;

namespace Calculator.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event)
    {
        throw new NotImplementedException();
    }
}