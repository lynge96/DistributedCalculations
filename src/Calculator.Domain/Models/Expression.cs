namespace Calculator.Domain.Models;

public sealed class Expression
{
    private const int MaxLength = 256;
    
    public string Value { get; }

    public Expression(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(Expression)} cannot be empty or null, please provide a valid expression");
        
        if (value.Length > MaxLength)
            throw new ArgumentException($"Expression is too long (max {MaxLength} characters)");

        Value = value;
    }
}