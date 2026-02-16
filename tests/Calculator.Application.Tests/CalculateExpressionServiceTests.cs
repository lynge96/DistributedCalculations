using Calculator.Application.Services;
using Calculator.Infrastructure;
using MathEvaluation;
using MathExpression = Calculator.Domain.Models.MathExpression;

namespace Calculator.Application.Tests;

public class CalculateExpressionServiceTests
{
    private readonly CalculateExpressionService _service;

    public CalculateExpressionServiceTests()
    {
        var calculator = new MathEvaluatorCalculator();
        _service = new CalculateExpressionService(calculator);
    }
    
    [Fact]
    public void Calculates_expression_correctly()
    {
        var expression = new MathExpression("2 + 3 * 5");

        var result = _service.Execute(expression);

        Assert.Equal(17m, result.Result);
    }

    [Fact]
    public void Invalid_expression_syntax_throws()
    {
        var expression = new MathExpression("2 + 3 *");

        Assert.Throws<MathExpressionException>(() =>
            _service.Execute(expression));
    }
}