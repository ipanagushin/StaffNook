#nullable disable
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;

namespace StaffNook.Infrastructure.Logging
{
    public abstract class ContextualLoggerBase : ILogger
    {
        private readonly ConcurrentDictionary<string, object> _context = new();
        private readonly Serilog.ILogger _log;
        private Stopwatch? _stopwatch;
        private static readonly Regex ExceptionRegex = new(@"at StaffNook\.([a-zA-Z0-9_\.]+)", RegexOptions.Compiled);

        protected ContextualLoggerBase(Serilog.ILogger log)
        {
            _log = log;
        }

        public void AddStopwatch(Stopwatch sw)
        {
            _stopwatch = sw;
        }

        public IDisposable UseContext(params (string, object?)[] contextVariables)
        {
            foreach (var (name, value) in contextVariables)
            {
                ForContext(name, value);
            }

            return new ContextVariablesDisposer(this, logger =>
            {
                foreach (var (name, _) in contextVariables)
                {
                    logger.RemoveContext(name);
                }
            });
        }

        public IDisposable UseContext(IReadOnlyDictionary<string, object> contextVariables)
        {
            return UseContext(contextVariables.Select(x => (x.Key, x.Value)).ToArray());
        }

        public ILogger ForContext(string name, object? value)
        {
            _context[name] = value;
            return this;
        }

        public ILogger RemoveContext(string name)
        {
            _context.TryRemove(name, out object _);
            return this;
        }

        public object? TryGetContextVariable(string name)
        {
            return _context.GetValueOrDefault(name);
        }

        public IReadOnlyDictionary<string, object?> CopyContext()
        {
            return _context.ToDictionary(x => x.Key, x => x.Value);
        }

        public void MergeContext(IReadOnlyDictionary<string, object?>? context)
        {
            if (context == null)
            {
                return;
            }

            foreach (var keyValuePair in context)
            {
                ForContext(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void Trace(string message, params object?[] parameters)
        {
            _log.ForContext(GetContextualVariables())
                .Verbose(message, parameters);
        }

        public void Verbose(string message, params object?[] parameters)
        {
            _log.ForContext(GetContextualVariables())
                .Verbose(message, parameters);
        }

        public void Info(string message, params object?[] parameters)
        {
            _log.ForContext(GetContextualVariables())
                .Information(message, parameters);
        }

        public void Write(LogLevel level, string? messageTemplate, dynamic? context)
        {
            var logger = _log.ForContext(GetContextualVariables());
            messageTemplate ??= "Empty message";

            var serilogLevel = MapLogLevel(level);
            logger.Write(serilogLevel, messageTemplate, context);
        }

        public void Write(Exception ex, LogLevel level, string? messageTemplate, dynamic? context)
        {
            var logger = _log.ForContext(GetContextualVariables());
            messageTemplate ??= "Empty message";

            var serilogLevel = MapLogLevel(level);
            logger.Write(serilogLevel, ex, messageTemplate, context);
        }

        public void Error(Exception? ex, string? message = null, params object?[] parameters)
        {
            var errorDescription = GetErrorDescription(ex, message);
            var errorId = GetErrorId(errorDescription);

            var contextVariables =
                GetContextualVariables(ex, ("errId", errorId), ("errorDescription", errorDescription));

            _log.ForContext(contextVariables)
                .Error(ex, message ?? ex?.Message, parameters);
        }

        public void Fatal(string message, params object?[] parameters)
        {
            var errorDescription = GetErrorDescription(null, message);
            var errorId = GetErrorId(errorDescription);

            _log.ForContext(GetContextualVariables(null, ("errId", errorId), ("errorDescription", errorDescription)))
                .Fatal(message, parameters);
        }

        public void Fatal(Exception ex, string message, params object?[] parameters)
        {
            var errorDescription = GetErrorDescription(ex, message);
            var errorId = GetErrorId(errorDescription);

            _log.ForContext(GetContextualVariables(ex, ("errId", errorId), ("errorDescription", errorDescription)))
                .Fatal(ex, message, parameters);
        }

        // Поможет потом для построения статистики повторяющихся ошибок
        private static string GetErrorId(string errorDescription)
        {
            return $"E{Math.Abs(StringHashHelper.GetStringHash(errorDescription)) % 1000000:000000}";
        }

        private static string GetErrorDescription(Exception? exception, string? message)
        {
            try
            {
                if (exception?.StackTrace != null)
                {
                    var exceptionType = exception.GetType().Name;
                    if (exceptionType != "Exception") exceptionType = exceptionType.Replace("Exception", "");

                    // если есть эксепшен, пробуем найти самое глубокое место в коде

                    // выбираем все фреймы, где есть код StaffNook, в трейсе оно такой вид будет иметь:
                    // StaffNook.Infrastructure.Exceptions.AuthorizationException: Ошибка авторизации. Некорректные данные. at StaffNook.Infrastructure.Implementations.Services.IdentityService.Login(LoginRequestDto requestDto) in <путь на машине>\src\StaffNook.Infrastructure\Implementations\Services\IdentityService.cs:line 40

                    var matches = ExceptionRegex.Matches(exception.StackTrace).Where(x => x.Groups[1].Success)
                        .Select(x => x.Groups[1].Value)
                        .ToArray();

                    if (matches.Any())
                        return $"{matches[0]}:{exceptionType}";

                    // если кода StaffNook нет (очень страно?), то просто берем код ошибки
                    return exceptionType;
                }

                // дальше случай, когда вызван .Error без эксепшена
                // чтобы определить где он вызван, используем StackTrace

                if (string.IsNullOrEmpty(message)) message = "(null)";
                if (message.Length > 20) message = message.Substring(0, 20);

                var stackTrace = new StackTrace();
                var frames = stackTrace.GetFrames();
                var frameMethod = frames
                    // пропускаем два фрейма (этот и Error)
                    .Skip(2)
                    .Select(x => x?.GetMethod())
                    .FirstOrDefault(x => x?.DeclaringType?.Namespace?.StartsWith("StaffNook.") == true);

                if (frameMethod != null)
                {
                    if (frameMethod.Name == "MoveNext")
                    {
                        // в случае async-метода, будет информация в виде RuntimeMethod,
                        // у него спефицичный формат записи, преобразуем в нормальный
                        var runtimeName = frameMethod.DeclaringType!.FullName!;
                        var name = Regex.Replace(runtimeName!, @"\+<(\w+)>.*", ".$1");
                        return $"{name}:{message}".Substring(8);
                    }

                    // здесь если не асинк
                    return $"{frameMethod.DeclaringType!.FullName}.{frameMethod.Name}:{message}"
                        .Substring(8);
                }

                return $"ER:{message}";
            }
            catch
            {
                return "ERRR";
            }
        }

        public void Error(string message, params object?[] parameters)
        {
            var errorDescription = GetErrorDescription(null, message);
            var errorId = GetErrorId(errorDescription);

            _log.ForContext(GetContextualVariables(null, ("errId", errorId), ("errorDescription", errorDescription)))
                .Error(message, parameters);
        }

        public void Warn(Exception? ex, string? message, params object?[] parameters)
        {
            _log.ForContext(GetContextualVariables(ex))
                .Warning(ex, message, parameters);
        }

        public void Warn(string message, params object?[] parameters)
        {
            _log.ForContext(GetContextualVariables())
                .Warning(message, parameters);
        }

        private IEnumerable<ILogEventEnricher> GetContextualVariables(Exception? ex = null,
            params (string, object)[] contextVariables)
        {
            var res = _context.Select(x => new PropertyEnricher(x.Key, x.Value)).ToList();
            if (_stopwatch != null)
            {
                res.Add(new PropertyEnricher("Elapsed", _stopwatch.ElapsedMilliseconds));
            }

            if (ex != null)
            {
                res.Add(new PropertyEnricher("ExceptionType", ex.GetType().FullName));
            }

            foreach (var (key, value) in contextVariables)
            {
                res.Add(new PropertyEnricher(key, value));
            }

            return res;
        }

        private static LogEventLevel MapLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Verbose => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Fatal => LogEventLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
    }
}