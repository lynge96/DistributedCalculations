using Calculator.Domain.Events;

namespace CalculationHistory.Worker.Interfaces;

public interface IHistoryStore
{
    void Add(CalculationCompletedEvent record);
    IReadOnlyList<CalculationCompletedEvent> GetHistory();
}