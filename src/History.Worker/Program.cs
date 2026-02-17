using History.Worker;
using Shared.Logging;
using Shared.RabbitMq;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.ConfigureSerilog("History.Worker");

builder.Services.AddSerilog();
builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddHostedService<ExpressionConsumer>();

var host = builder.Build();
host.Run();