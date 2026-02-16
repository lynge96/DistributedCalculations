using Calculator.Domain.Interfaces;
using Calculator.Domain.Models;
using MathEvaluation.Extensions;

namespace Calculator.Infrastructure;

public sealed class MathEvaluatorCalculator : ICalculator
{
    public decimal Calculate(MathExpression mathExpression)
    {
        var result = mathExpression.Value.EvaluateDecimal();
        
        return result;
    }
}