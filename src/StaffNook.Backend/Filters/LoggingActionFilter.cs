using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StaffNook.Infrastructure.Converters;
using StaffNook.Infrastructure.Logging;

namespace StaffNook.Backend.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private readonly ContextualLogger _logger;

        private readonly HashSet<string> _methodsToLogResult = new(new[] { "post", "put" });

        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            Formatting = Formatting.None,
            Converters = { new StringEnumConverter(), new CancellationTokenRequestConverter() },
            ContractResolver = new ShouldSerializeContractResolver()
        };

        public LoggingActionFilter(ContextualLogger logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var route = context.ActionDescriptor.AttributeRouteInfo?.Template ?? "";
            
            // ToDo:: дополнить данными из клеймов токена
            EnrichUserContext(context, ClaimTypes.NameIdentifier, LoggerConst.UserId);

            var sw = Stopwatch.StartNew();

            _logger.ForContext(LoggerConst.RequestId, context.HttpContext.TraceIdentifier);

            _logger.ForContext("action", context.ActionDescriptor.DisplayName);
            _logger.Info("request started {methodName} {routeTemplateName} {arguments}", method, route,
                JsonConvert.SerializeObject(context.ActionArguments, JsonSerializerSettings));

            try
            {
                var executedContext = await next();

                if (executedContext.Exception != null)
                {
                    var exceptionMessage =
                        $"{executedContext.Exception.GetType().Name}: {executedContext.Exception.Message}";
                    _logger.ForContext(LoggerConst.Exception, exceptionMessage);
                }

                if (!context.HttpContext.Response.HasStarted)
                {
                    if (!context.HttpContext.Response.Headers.ContainsKey(LoggerConst.RequestId))
                    {
                        context.HttpContext.Response.Headers.TryAdd(LoggerConst.RequestId,
                            context.HttpContext.TraceIdentifier);
                    }
                }

                var objResult = executedContext.Result as ObjectResult;
                if (objResult?.Value is ProblemDetails pd)
                {
                    pd.Extensions.Add(LoggerConst.RequestId, context.HttpContext.TraceIdentifier);
                }

                if (_methodsToLogResult.Contains(context.HttpContext.Request.Method.ToLower())
                    && executedContext.Result is ObjectResult objectResult)
                {
                    var response = JsonConvert.SerializeObject(objectResult.Value, JsonSerializerSettings);
                    if (response.Length > 1000) response = response.Substring(0, 1000);
                    _logger.ForContext(LoggerConst.Response, response);
                }

                context.HttpContext.Items["Timer"] = sw;
            }
            catch (Exception e)
            {
                _logger.Error(e, "request error {methodName} {routeTemplateName} {totalRequestElapsed}", method, route,
                    sw.ElapsedMilliseconds);
            }
        }

        private void EnrichUserContext(ActionContext context, string claimName, string name = null)
        {
            var claim = context.HttpContext.User.FindFirst(claimName);
            if (claim is not null)
            {
                _logger.ForContext(name ?? claimName, claim.Value);
            }
        }
    }
}