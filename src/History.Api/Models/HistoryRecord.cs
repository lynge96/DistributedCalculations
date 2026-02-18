namespace History.Api.Models;

public sealed class HistoryRecord(
    Guid CalculationId,
    string Expression,
    decimal Result,
    DateTimeOffset OccurredAt
);