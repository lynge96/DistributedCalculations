using CalculationHistory.Worker;
using Serilog;
using Shared.Logging;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.ConfigureSerilog();

builder.Services.AddSerilog();
builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddHostedService<ExpressionConsumer>();
builder.Services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

var host = builder.Build();
host.Run();