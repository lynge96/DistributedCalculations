namespace Calculator.Domain.Models;

public sealed record MathExpression
{
    public string Value { get; }

    public MathExpression(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(MathExpression)} cannot be empty or null, please provide a valid expression");

        Value = value;
    }
}