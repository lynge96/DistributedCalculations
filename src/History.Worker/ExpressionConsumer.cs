using System.Text;
using Calculator.Domain.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMq;

namespace History.Worker;

public class ExpressionConsumer : BackgroundService
{
    private readonly ILogger<ExpressionConsumer> _logger;
    private readonly IOptions<RabbitMqOptions> _options;

    public ExpressionConsumer(
        ILogger<ExpressionConsumer> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var factory = new ConnectionFactory { HostName = _options.Value.Host, Port = _options.Value.Port, UserName = _options.Value.Username, Password = _options.Value.Password };

            await using var connection = await factory.CreateConnectionAsync(ct);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: ct);
            
            await channel.QueueDeclareAsync(queue: "calculations", durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: ct);
            
            Console.WriteLine(" [*] Waiting for messages.");
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation(" [x] Received {Message}", message);
                return Task.CompletedTask;
            };
            
            await channel.BasicConsumeAsync("calculations", true, consumer, ct);
            
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