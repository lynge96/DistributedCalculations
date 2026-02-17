namespace Calculator.Domain.Exceptions;

public sealed class CalculationOverflowException : Exception
{
    public CalculationOverflowException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}