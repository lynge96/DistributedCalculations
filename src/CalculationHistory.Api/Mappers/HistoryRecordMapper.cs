using CalculationHistory.Worker.DTOs;
using Calculator.Domain.Events;

namespace CalculationHistory.Worker.Mappers;

public static class HistoryRecordMapper
{
    public static CalculationHistoryDto ToDto(this IReadOnlyList<CalculationCompletedEvent> records)
    {
        return new CalculationHistoryDto { Records = records.ToList() };
    }
}