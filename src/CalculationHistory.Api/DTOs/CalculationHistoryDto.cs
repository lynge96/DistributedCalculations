using Calculator.Domain.Events;

namespace CalculationHistory.Worker.DTOs;

public sealed record CalculationHistoryDto
{
    public int Count => CalculationHistory?.Count ?? 0;
    public List<CalculationCompletedEvent>? CalculationHistory { get; init; }
}
