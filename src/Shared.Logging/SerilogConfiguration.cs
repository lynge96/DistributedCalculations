using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shared.Logging;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this IConfiguration configuration, string serviceName)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Service", serviceName)
            .CreateLogger();
    }
}