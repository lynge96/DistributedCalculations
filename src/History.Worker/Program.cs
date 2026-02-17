using History.Worker;
using Shared.Logging;
using Shared.RabbitMq;
using Serilog;
using Shared.RabbitMq.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.ConfigureSerilog("History.Worker");

builder.Services.AddSerilog();
builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddHostedService<ExpressionConsumer>();
builder.Services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

var host = builder.Build();
host.Run();