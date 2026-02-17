using Calculator.Domain.Events;

namespace History.Worker;

public class HistoryConsumer : BackgroundService
{
    private readonly ILogger<HistoryConsumer> _logger;

    public HistoryConsumer(ILogger<HistoryConsumer> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            await Task.Delay(1000, stoppingToken);
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