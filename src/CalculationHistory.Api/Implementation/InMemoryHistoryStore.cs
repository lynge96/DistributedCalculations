using CalculationHistory.Worker.Interfaces;
using Calculator.Domain.Events;
using Microsoft.Extensions.Options;
using Shared.RabbitMq;

namespace CalculationHistory.Worker.Implementation;

public sealed class InMemoryHistoryStore : IHistoryStore
{
    private readonly LinkedList<CalculationCompletedEvent> _items = [];
    private readonly ILogger<InMemoryHistoryStore> _logger;
    private readonly int _recordsInHistory;
    
    public InMemoryHistoryStore(
        ILogger<InMemoryHistoryStore> logger,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _recordsInHistory = options.Value.RecordsToKeep;
    }
    
    public void Add(CalculationCompletedEvent record)
    {
        _logger.LogInformation("Saving Calculation Event: {Id}", record.CalculationId);
        
        _items.AddFirst(record);

        while (_items.Count > _recordsInHistory)
        {
            _logger.LogWarning("History store is full. Removing last item: {Record}", _items.Last?.Value);
            _items.RemoveLast();
        }
    }

    public IReadOnlyList<CalculationCompletedEvent> GetHistory()
    {
        _logger.LogInformation("Getting {Count} records from history", _items.Count);
        return _items.ToList();
    }

    public void Clear()
    {
        _logger.LogInformation("Clearing {Count} records from history", _items.Count);
        _items.Clear();
    }
}