using Calculator.Domain.Exceptions;
using Calculator.Domain.Models;
using Calculator.Infrastructure.MathEvaluation;
using MathEvaluation;
using Microsoft.Extensions.Logging.Abstractions;

namespace Calculator.Infrastructure.Tests;

public class MathEvaluationCalculatorTests
{
    private readonly MathEvaluationCalculator _calculator;
    public MathEvaluationCalculatorTests()
    {
        var logger = NullLogger<MathEvaluationCalculator>.Instance;
        _calculator = new MathEvaluationCalculator(logger);
    }
    
    [Fact]
    public void Calculates_complex_expression_correctly()
    {
        var expression = new Expression("2 + 3 * (4 - 1)");

        var result = _calculator.Calculate(expression);

        Assert.Equal(11m, result);
    }
    
    [Fact]
    public void Throws_overflow_exception_when_result_is_too_large()
    {
        var expression = new Expression("999999999999999999999 * 999999999999999999");

        Assert.Throws<CalculationOverflowException>(() =>
            _calculator.Calculate(expression));
    }
    
    [Fact]
    public void Invalid_expression_syntax_throws()
    {
        var expression = new Expression("2 + 3 *");

        Assert.Throws<MathExpressionException>(() =>
            _calculator.Calculate(expression));
    }
}