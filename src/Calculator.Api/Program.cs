using Calculator.Api;
using Calculator.Api.Endpoints;
using Calculator.Api.Middleware;
using Serilog;
using Shared.Logging;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCalculator();
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

builder.Configuration.ConfigureSerilog("Calculator.Api");

builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("Development");
app.UseHttpsRedirection();
app.UseRouting();
app.MapCalculationsEndpoints();

app.Run();
