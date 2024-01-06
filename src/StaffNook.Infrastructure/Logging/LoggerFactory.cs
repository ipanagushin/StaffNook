namespace StaffNook.Infrastructure.Logging
{
    public abstract class LoggerFactory
    {
        public abstract ILogger GetLogger(string className);
    }
}