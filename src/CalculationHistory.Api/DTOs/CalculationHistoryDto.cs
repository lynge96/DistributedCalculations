using Calculator.Domain.Events;

namespace CalculationHistory.Worker.DTOs;

public sealed record CalculationHistoryDto
{
    public int Count => Records?.Count ?? 0;
    public List<CalculationCompletedEvent>? Records { get; init; }
}
