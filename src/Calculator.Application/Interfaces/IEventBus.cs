namespace Calculator.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event);
}