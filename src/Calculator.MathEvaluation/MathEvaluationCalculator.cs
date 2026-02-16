using Calculator.Domain.Exceptions;
using Calculator.Domain.Interfaces;
using Calculator.Domain.Models;
using MathEvaluation;
using MathEvaluation.Context;
using Microsoft.Extensions.Logging;

namespace Calculator.Infrastructure;

public sealed class MathEvaluationCalculator : ICalculator
{
    private readonly ILogger<MathEvaluationCalculator> _logger;
    
    public MathEvaluationCalculator(ILogger<MathEvaluationCalculator> logger)
    {
        _logger = logger;
    }
    
    public decimal Calculate(Expression expression)
    {
        try
        {
            using var calculation = new MathExpression(expression.Value, new ScientificMathContext());

            calculation.Evaluating += (_, args) =>
            {
                _logger.LogInformation("{0}: {1} = {2};{3}",
                    args.Step,
                    args.MathString[args.Start..(args.End + 1)],
                    args.Value,
                    args.IsCompleted ? "\nCompleted" : string.Empty);
            };

            var result = calculation.EvaluateDecimal();
            
            return result;

        }
        catch (MathExpressionException ex) when (IsOverflow(ex))
        {
            throw new CalculationOverflowException("The calculation result exceeds supported numeric range", ex);
        }
    }

    private static bool IsOverflow(MathExpressionException ex)
    {
        return ex.Message.Contains("overflow", StringComparison.OrdinalIgnoreCase)
               || ex.InnerException is OverflowException;
    }
}