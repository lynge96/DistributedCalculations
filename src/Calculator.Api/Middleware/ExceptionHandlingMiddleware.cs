using System.Net;
using System.Text.Json;
using MathEvaluation;

namespace Calculator.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var path = context.Request.Path;
        var traceId = context.TraceIdentifier;

        ErrorContext errorContext;

        switch (exception)
        {
            case ArgumentException argEx:
            {
                errorContext = new ErrorContext(argEx.GetType().Name, (int)HttpStatusCode.BadRequest, path.Value, traceId, argEx.Message);
                
                _logger.LogWarning(argEx, "Bad request: {ErrorMessage}", errorContext.ErrorMessage);
                
                context.Response.StatusCode = errorContext.StatusCode;
                break;
            }
            case MathExpressionException mathEx:
            {
                errorContext = new ErrorContext(mathEx.GetType().Name, (int)HttpStatusCode.BadRequest, path.Value, traceId, mathEx.Message);
                
                _logger.LogWarning(mathEx, "Invalid expression: {ErrorMessage}", errorContext.ErrorMessage);
                
                context.Response.StatusCode = errorContext.StatusCode;
                break;
            }
            default:
            {
                errorContext = new ErrorContext("UnhandledException", (int)HttpStatusCode.InternalServerError, path.Value, traceId);
                
                _logger.LogError(exception, "Unhandled exception: {ErrorMessage}", errorContext.ErrorMessage);
                
                context.Response.StatusCode = errorContext.StatusCode;
                break;
            }
        }

        var response = new ErrorResponse(errorContext.StatusCode, errorContext.Type, errorContext.ErrorMessage ?? "An error occurred while processing your request.");
        
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private record ErrorContext(string Type, int StatusCode, string? Path, string TraceId, string? ErrorMessage = null);
    private record ErrorResponse(int StatusCode, string Type, string Message);
}