using Calculator.Application.Interfaces;
using Calculator.Application.Services;
using Calculator.Domain.Interfaces;
using Calculator.Infrastructure;
using Calculator.Infrastructure.MathEvaluation;
using Calculator.Infrastructure.RabbitMq;

namespace Calculator.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services)
    {
        services.AddScoped<ICalculator, MathEvaluationCalculator>();
        services.AddScoped<IEventBus, RabbitMqEventBus>();
        services.AddScoped<CalculateExpressionService>();

        return services;
    }
}