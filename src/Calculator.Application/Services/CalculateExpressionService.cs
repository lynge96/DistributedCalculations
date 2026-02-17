using Calculator.Application.Interfaces;
using Calculator.Application.Records;
using Calculator.Domain.Events;
using Calculator.Domain.Interfaces;
using Calculator.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Calculator.Application.Services;

public sealed class CalculateExpressionService
{
    private readonly ICalculator _calculator;
    private readonly IEventBus _eventBus;
    private readonly ILogger<CalculateExpressionService> _logger;
    
    public CalculateExpressionService(
        ICalculator calculator,
        IEventBus eventBus,
        ILogger<CalculateExpressionService> logger)
    {
        _calculator = calculator;
        _eventBus = eventBus;
        _logger = logger;
    }

    public CalculationResult Execute(Expression expression)
    {
        _logger.LogInformation(
            "Executing calculation for expression: {Expression}", 
            expression.Value);
        
        var result = _calculator.Calculate(expression);
        
        var calculationResult = new CalculationResult(result);

        // _eventBus.PublishAsync(new CalculationCompletedEvent(calculationResult.CalculationId, expression.Value, calculationResult.Result, calculationResult.Timestamp));
        
        _logger.LogInformation(
            "Calculation result: {Result}", 
            new { Result = calculationResult.Result, Id = calculationResult.CalculationId } );
        
        return calculationResult;
    }
}