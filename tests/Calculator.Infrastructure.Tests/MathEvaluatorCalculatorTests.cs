using MathExpression = Calculator.Domain.Models.MathExpression;

namespace Calculator.Infrastructure.Tests;

public class MathEvaluatorCalculatorTests
{
    [Fact]
    public void Calculates_complex_expression_correctly()
    {
        var calc = new MathEvaluatorCalculator();
        var expression = new MathExpression("2 + 3 * (4 - 1)");

        var result = calc.Calculate(expression);

        Assert.Equal(11m, result);
    }
}