using System.Dynamic;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace StaffNook.Infrastructure.Logging
{
    public class DefaultLoggerFactory : LoggerFactory
    {
        public static readonly Lazy<Serilog.ILogger> LoggerFactory = new(() => GetConfiguration().CreateLogger());

        private static LoggerConfiguration GetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new LoggerConfiguration();

            configuration = configuration
                .Destructure.ByTransforming<ExpandoObject>(e => new Dictionary<string, object>(e))
                .Enrich.WithProperty(LoggerConst.Environment, environment)
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning);

            // Добавляем spanId и traceId для поиска по логам
            configuration.Enrich.With<LogTelemetryEnricher>();

            configuration = configuration
                .WriteTo.Console(theme: SystemConsoleTheme.Literate,
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}");

            return configuration;
        }

        public override ILogger GetLogger(string className)
        {
            return ContextualLogger.Create(LoggerFactory.Value).ForContext(nameof(className), className);
        }
    }
}