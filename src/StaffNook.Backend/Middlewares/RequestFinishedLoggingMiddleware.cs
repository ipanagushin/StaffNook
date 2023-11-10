using System.Diagnostics;
using StaffNook.Infrastructure.Logging;
using ILogger = StaffNook.Infrastructure.Logging.ILogger;

namespace StaffNook.Backend.Middlewares
{
    public class RequestFinishedLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestFinishedLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger)
        {
            if (!context.Response.Headers.ContainsKey(LoggerConst.RequestId))
            {
                context.Response.Headers.Add(LoggerConst.RequestId, context.TraceIdentifier);
            }

            await _next(context);
            var method = context.Request.Method;
            var route = context.Items["RouteTemplate"] as string;
            if (string.IsNullOrEmpty(route))
            {
                route = context.Request.Path.Value;
            }

            var requestStatusCode = context.Response.StatusCode.ToString();
            var requestStatus = requestStatusCode.First() switch
            {
                '1' => "Informational",
                '2' => "Success",
                '3' => "Redirection",
                '4' => "Client Error",
                '5' => "Server Error",
                _ => "Unknown"
            };
            
            Stopwatch? sw = null;
            if (context.Items.TryGetValue("Timer", out var item))
            {
                sw = (Stopwatch?)item;
            }

            logger.Info(
                "request finished {methodName} {routeTemplateName} {totalRequestElapsed} {requestStatus} {requestStatusCode}",
                method, route, sw?.ElapsedMilliseconds ?? 0, requestStatus, requestStatusCode);
            
            sw?.Stop();
        }
    }
}