namespace Calculator.Application.Records;

public sealed class CalculationResult
{
    public Guid CalculationId { get; init; }
    public decimal Result { get; }
    public DateTimeOffset Timestamp { get; init; }

    public CalculationResult(decimal result)
    {
        Result = result;
        Timestamp = DateTimeOffset.UtcNow;
        CalculationId = Guid.NewGuid();
    }
}