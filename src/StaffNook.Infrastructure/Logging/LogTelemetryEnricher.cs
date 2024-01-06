using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace StaffNook.Infrastructure.Logging
{
    public class LogTelemetryEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            if (activity is not null && activity.IsAllDataRequested)
            {
                AddTraceId(logEvent, activity);
                AddSpanId(logEvent, activity);
            }
        }

        private static void AddSpanId(LogEvent logEvent, Activity activity)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(LoggerConst.SpanId,
                new ScalarValue(activity.Context.SpanId.ToString())));
        }

        private static void AddTraceId(LogEvent logEvent, Activity activity)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(LoggerConst.TraceId,
                new ScalarValue(activity.Context.TraceId.ToString())));
        }
    }
}