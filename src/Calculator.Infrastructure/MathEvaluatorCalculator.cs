using Calculator.Domain.Interfaces;
using MathEvaluation;
using MathEvaluation.Context;
using Microsoft.Extensions.Logging;
using MathExpression = Calculator.Domain.Models.MathExpression;

namespace Calculator.Infrastructure;

public sealed class MathEvaluatorCalculator : ICalculator
{
    private readonly ILogger<MathEvaluatorCalculator> _logger;
    
    public MathEvaluatorCalculator(ILogger<MathEvaluatorCalculator> logger)
    {
        _logger = logger;
    }
    
    public decimal Calculate(MathExpression mathExpression)
    {
        using var expression = new MathEvaluation.MathExpression(mathExpression.Value, new ScientificMathContext());

        expression.Evaluating += (_, args) =>
        {
            _logger.LogInformation("{0}: {1} = {2};{3}",
                args.Step,
                args.MathString[args.Start..(args.End + 1)],
                args.Value,
                args.IsCompleted ? "\nCompleted" : string.Empty);
        };

        var result = expression.EvaluateDecimal();
        
        return result;
    }

}