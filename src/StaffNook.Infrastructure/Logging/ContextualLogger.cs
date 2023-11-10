namespace StaffNook.Infrastructure.Logging;

public class ContextualLogger : ContextualLoggerBase
{
    public ContextualLogger()
        : base(DefaultLoggerFactory.LoggerFactory.Value)
    {
    }

    private ContextualLogger(Serilog.ILogger logger) : base(logger)
    {
    }

    public static ContextualLogger CreateDefault()
    {
        return new ContextualLogger(DefaultLoggerFactory.LoggerFactory.Value);
    }

    public static ContextualLogger Create(Serilog.ILogger logger)
    {
        return new ContextualLogger(DefaultLoggerFactory.LoggerFactory.Value);
    }
}