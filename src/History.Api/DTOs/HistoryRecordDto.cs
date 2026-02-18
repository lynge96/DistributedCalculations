namespace History.Api.DTOs;

public sealed record HistoryRecordDto (
    Guid CalculationId,
    string Expression,
    decimal Result,
    DateTimeOffset OccurredAt
);