using Microsoft.Extensions.Logging.Abstractions;
using MathExpression = Calculator.Domain.Models.MathExpression;

namespace Calculator.Infrastructure.Tests;

public class MathEvaluatorCalculatorTests
{
    private readonly MathEvaluatorCalculator _calculator;
    public MathEvaluatorCalculatorTests()
    {
        var logger = NullLogger<MathEvaluatorCalculator>.Instance;
        _calculator = new MathEvaluatorCalculator(logger);
    }
    
    [Fact]
    public void Calculates_complex_expression_correctly()
    {
        var expression = new MathExpression("2 + 3 * (4 - 1)");

        var result = _calculator.Calculate(expression);

        Assert.Equal(11m, result);
    }
}