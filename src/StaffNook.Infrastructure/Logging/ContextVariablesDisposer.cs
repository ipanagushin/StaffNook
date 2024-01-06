namespace StaffNook.Infrastructure.Logging
{
    internal class ContextVariablesDisposer : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Action<ILogger> _onExit;

        internal ContextVariablesDisposer(ILogger logger, Action<ILogger> onExit)
        {
            _logger = logger;
            _onExit = onExit;
        }

        public void Dispose()
        {
            _onExit(_logger);
        }
    }
}