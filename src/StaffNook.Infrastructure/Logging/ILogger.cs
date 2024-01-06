#nullable enable
using System.Diagnostics;

namespace StaffNook.Infrastructure.Logging
{
    public interface ILogger
    {
        IDisposable UseContext(params (string, object?)[] contextVariables);
        IDisposable UseContext(IReadOnlyDictionary<string, object> contextVariables);
        ILogger ForContext(string name, object? value);
        ILogger RemoveContext(string name);
        object? TryGetContextVariable(string name);

        /// <summary>
        /// Для логов, описывающих детали обработки, которые могут потребоваться для расследования багов.
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        /// <param name="parameters">Параметры, подставляемые в сообщение</param>
        void Trace(string message, params object?[] parameters);

        /// <summary>
        /// Для часто вызываемых логов, описывающих детали обработки,
        /// которые могут потребоваться для расследования багов.
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        /// <param name="parameters">Параметры, подставляемые в сообщение</param>
        void Verbose(string message, params object?[] parameters);

        /// <summary>
        /// Для описания основных этапов в обработке данных.
        /// Подробности должны описываться при помощи <see cref="Trace"/> или <see cref="Verbose"/>.
        /// </summary>
        /// <remarks>
        /// Например, начало и конец создания коров. Смена статуса, результаты обработки и т.д.
        /// </remarks>
        /// <param name="message">Логируемое сообщение</param>
        /// <param name="parameters">Параметры, подставляемые в сообщение</param>
        void Info(string message, params object?[] parameters);

        void Error(Exception ex, string? message = null, params object?[] parameters);
        void Error(string message, params object?[] parameters);
        void Warn(Exception ex, string? message, params object?[] parameters);
        void Warn(string message, params object?[] parameters);
        void Fatal(string message, params object?[] parameters);
        void Fatal(Exception ex, string message, params object?[] parameters);
        IReadOnlyDictionary<string, object?> CopyContext();
        void MergeContext(IReadOnlyDictionary<string, object?>? context);
        void AddStopwatch(Stopwatch sw);
        void Write(LogLevel level, string? messageTemplate, dynamic arguments);
        void Write(Exception ex, LogLevel level, string? messageTemplate, dynamic arguments);
    }
}