using System.Text;
using System.Text.Json;
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
    private readonly string _queueName;

    public ExpressionConsumer(
        ILogger<ExpressionConsumer> logger,
        IRabbitMqConnectionFactory factory,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _factory = factory;
        _queueName = options.Value.QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await using var channel = await _factory.CreateChannelAsync(ct);

            _logger.LogInformation("Waiting for messages on '{queue}' queue.", _queueName);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                
                var receivedEvent = JsonSerializer.Deserialize<CalculationCompletedEvent>(json);

                _logger.LogInformation("Received event: {@Event}", receivedEvent);

                if (receivedEvent != null) 
                    Handle(receivedEvent);

                await Task.Yield();
            };

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer, cancellationToken: ct);
            
            await Task.Delay(1000, ct);
        }
    }

    private void Handle(object @event)
    {
        if (@event is CalculationCompletedEvent calculation)
        {
            _logger.LogInformation(
                "Saving calculation {Id}: {Expression} = {Result} - Timestamp: {Timestamp}",
                calculation.CalculationId, calculation.Expression, calculation.Result, calculation.Timestamp);
            
            // Save calculation in memory
        }
    }
}