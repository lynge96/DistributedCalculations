using History.Worker;
using Shared.Logging;

var builder = Host.CreateApplicationBuilder(args);

SerilogConfiguration.Configure(builder.Configuration, "History.Worker");

builder.Services.AddHostedService<HistoryConsumer>();

var host = builder.Build();
host.Run();