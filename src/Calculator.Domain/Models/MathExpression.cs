namespace Calculator.Domain.Models;

public sealed record MathExpression
{
    public string Value { get; }

    public MathExpression(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Expression cannot be empty");

        Value = value;
    }
}