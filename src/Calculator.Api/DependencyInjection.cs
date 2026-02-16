using Calculator.Application.Services;
using Calculator.Domain.Interfaces;
using Calculator.Infrastructure;

namespace Calculator.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services)
    {
        services.AddScoped<ICalculator, MathEvaluationCalculator>();
        services.AddScoped<CalculateExpressionService>();

        return services;
    }
}