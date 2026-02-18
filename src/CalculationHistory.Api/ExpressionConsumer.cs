using System.Text;
using System.Text.Json;
using CalculationHistory.Worker.Interfaces;
using Calculator.Domain.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;

namespace CalculationHistory.Worker;

public class ExpressionConsumer : BackgroundService
{
    private readonly ILogger<ExpressionConsumer> _logger;
    private readonly IRabbitMqConnectionFactory _factory;
    private readonly IHistoryStore _historyStore;
    private readonly string _queueName;

    public ExpressionConsumer(
        ILogger<ExpressionConsumer> logger,
        IRabbitMqConnectionFactory factory,
        IHistoryStore historyStore,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _factory = factory;
        _historyStore = historyStore;
        _queueName = options.Value.QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var channel = await _factory.CreateChannelAsync(ct);
        _logger.LogInformation("Waiting for messages on '{queue}' queue.", _queueName);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var receivedEvent = JsonSerializer.Deserialize<CalculationCompletedEvent>(json);
                
                if (receivedEvent is null)
                {
                    _logger.LogWarning("Received null/unparseable event. Nacking message.");
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: ct);
                    return;
                }
                
                _logger.LogInformation("Received event: {@Event}", receivedEvent);

                _historyStore.Add(receivedEvent);
                
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed processing message. Nacking (requeue=true).");
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: ct);
            }
        };

        await channel.BasicConsumeAsync(
            queue: _queueName, 
            autoAck: false, 
            consumer: consumer,
            cancellationToken: ct);
        
        await Task.Delay(Timeout.Infinite, ct);
    }

    private void Handle(CalculationCompletedEvent calculation)
    {
        _logger.LogInformation(
            "Saving calculation {Id}: {Expression} = {Result} - Timestamp: {Timestamp}",
            calculation.CalculationId, calculation.Expression, calculation.Result, calculation.Timestamp);

        _historyStore.Add(calculation);
    }
}