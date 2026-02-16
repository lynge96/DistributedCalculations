namespace Calculator.Api.Dtos;

public sealed record CalculationResponseDto(Guid CalculationId, decimal Result, string MathExpression, DateTimeOffset Timestamp);