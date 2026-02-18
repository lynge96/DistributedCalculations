using Calculator.Api.Dtos;
using Calculator.Application.Services;
using Calculator.Domain.Models;

namespace Calculator.Api.Endpoints;

public static class CalculationsEndpoints
{
    public static void MapCalculationsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/calculations", (CalculateRequestDto request, CalculateExpressionService calculationService) =>
        {
            var expression = new Expression(request.MathExpression);
            var result = calculationService.Execute(expression);

            var responseDto = new CalculationResponseDto(result.CalculationId, result.Result, request.MathExpression, result.Timestamp);
            
            return Results.Ok(responseDto);
        });
    }
}