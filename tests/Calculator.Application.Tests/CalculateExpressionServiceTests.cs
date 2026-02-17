using Calculator.Application.Interfaces;
using Calculator.Application.Services;
using Calculator.Domain.Interfaces;
using Calculator.Domain.Models;
using Calculator.Infrastructure;
using Calculator.Infrastructure.MathEvaluation;
using MathEvaluation;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Calculator.Application.Tests;

public class CalculateExpressionServiceTests
{
    private readonly CalculateExpressionService _service;

    public CalculateExpressionServiceTests()
    {
        var calculator = Substitute.For<ICalculator>();
        var eventBus = Substitute.For<IEventBus>();
        var logger = NullLogger<CalculateExpressionService>.Instance;

        _service = new CalculateExpressionService(calculator, eventBus, logger);
    }
    
    [Fact]
    public void Calculates_expression_correctly()
    {
        var logger = NullLogger<MathEvaluationCalculator>.Instance;
        var calculator = new MathEvaluationCalculator(logger);

        var expression = new Expression("2 + 3 * 5");

        var result = calculator.Calculate(expression);

        Assert.Equal(17m, result);
    }
}