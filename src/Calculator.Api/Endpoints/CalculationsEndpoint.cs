using Calculator.Api.Dtos;
using Calculator.Application.Services;
using Calculator.Domain.Models;

namespace Calculator.Api.Endpoints;

public static class CalculationsEndpoint
{
    public static void MapCalculationsEndpoints(this WebApplication app)
    {
        app.MapPost("/calculations", (CalculateRequestDto request, CalculateExpressionService calculationService) =>
        {
            var expression = new MathExpression(request.MathExpression);
            var result = calculationService.Execute(expression);

            var responseDto = new CalculationResponseDto(result.CalculationId, result.Result, request.MathExpression, result.Timestamp);
            return Results.Ok(responseDto);
        });
    }
}