namespace Calculator.Domain.Events;

public sealed record CalculationCompletedEvent(
    Guid CalculationId,
    string Expression,
    decimal Result,
    DateTimeOffset Timestamp
);