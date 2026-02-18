using History.Api.Models;

namespace History.Api.Interfaces;

public interface IHistoryStore
{
    void Add(HistoryRecord record);
    IReadOnlyList<HistoryRecord> GetHistory();
}