using History.Api.Interfaces;
using History.Api.Models;

namespace History.Api.Implementation;

public sealed class InMemoryHistoryStore : IHistoryStore
{
    private readonly LinkedList<HistoryRecord> _items = new();
    
    public void Add(HistoryRecord record)
    {
        _items.AddFirst(record);

        while (_items.Count > 5)
            _items.RemoveLast();
    }

    public IReadOnlyList<HistoryRecord> GetHistory()
    {
        return _items.ToList();
    }
}