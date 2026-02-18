using CalculationHistory.Worker;
using CalculationHistory.Worker.Endpoints;
using CalculationHistory.Worker.Implementation;
using CalculationHistory.Worker.Interfaces;
using Serilog;
using Shared.Logging;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Configuration.ConfigureSerilog();
builder.Services.AddSerilog();

builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddHostedService<ExpressionConsumer>();
builder.Services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddSingleton<IHistoryStore, InMemoryHistoryStore>();

var app = builder.Build();
app.UseCors("Development");
app.UseHttpsRedirection();
app.UseRouting();
app.MapHistoryEndpoints();

app.Run();