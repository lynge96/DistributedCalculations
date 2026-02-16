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
            var expression = new MathExpression(request.Expression);
            var result = calculationService.Execute(expression);

            return Results.Ok(new CalculationResponseDto(Guid.NewGuid(), result.Result));
        });
    }
}