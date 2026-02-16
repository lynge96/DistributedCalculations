using Calculator.Application.Records;
using Calculator.Domain.Interfaces;
using Calculator.Domain.Models;

namespace Calculator.Application.Services;

public sealed class CalculateExpressionService
{
    private readonly ICalculator _calculator;
    
    public CalculateExpressionService(ICalculator calculator)
    {
        _calculator = calculator;
    }

    public CalculationResult Execute(MathExpression mathExpression)
    {
        var result = _calculator.Calculate(mathExpression);
        
        return new CalculationResult(result);
    }
}