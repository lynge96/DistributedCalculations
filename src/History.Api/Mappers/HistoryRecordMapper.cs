using History.Api.DTOs;
using History.Api.Models;

namespace History.Api.Mappers;

public static class HistoryRecordMapper
{
    public static HistoryRecord ToDomain(this HistoryRecordDto dto) => new(
        dto.CalculationId, dto.Expression, dto.Result, dto.OccurredAt
    );
}