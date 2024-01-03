using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StaffNook.Backend.Filters.Helpers;
using StaffNook.Infrastructure.Exceptions;
using ILogger = StaffNook.Infrastructure.Logging.ILogger;

namespace StaffNook.Backend.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            dynamic response = new ExpandoObject();
            int statusCode;
            switch (context.Exception)
            {
                case AuthenticationException authenticationException:
                    response = new
                    {
                        authenticationException.Message
                    };
                    statusCode = 403;
                    break;
                case AuthorizationException authorizationException:
                    response = new
                    {
                        authorizationException.Message
                    };
                    statusCode = 400;
                    break;
                case BadHttpRequestException badHttpRequestException:
                {
                    if (badHttpRequestException.StatusCode == (int)HttpStatusCode.RequestEntityTooLarge)
                    {
                        var (requestSize, requestUnit) = (
                            context.GetRequestContentSize(RequestSizePolicyHelper.DataUnit.MiB),
                            RequestSizePolicyHelper.DataUnit.MiB);
                        var (maxRequestSize, maxRequestUnit) = (
                            context.GetRequestBodySizeLimit(RequestSizePolicyHelper.DataUnit.MiB),
                            RequestSizePolicyHelper.DataUnit.MiB);
                        if (requestSize < 1)
                        {
                            (requestSize, requestUnit) = (
                                context.GetRequestContentSize(RequestSizePolicyHelper.DataUnit.KiB),
                                RequestSizePolicyHelper.DataUnit.KiB);
                        }

                        if (maxRequestSize < 1)
                        {
                            (maxRequestSize, maxRequestUnit) = (
                                context.GetRequestBodySizeLimit(RequestSizePolicyHelper.DataUnit.KiB),
                                RequestSizePolicyHelper.DataUnit.KiB);
                        }

                        statusCode = 413;
                        response = new
                        {
                            Message =
                                $"Размер запроса или файла слишком большой ({requestSize} {requestUnit}). Он не должен превышать {maxRequestSize} {maxRequestUnit}"
                        };
                    }
                    else
                    {
                        response = GetDefaultResponse(context);
                        statusCode = 500;
                    }

                    break;
                }
                case NotFoundException validationException:
                {
                    response = new
                    {
                        validationException.Message
                    };
                    statusCode = 404;
                    break;
                }
                case BusinessException validationException:
                {
                    response = new
                    {
                        validationException.Message
                    };
                    statusCode = 400;
                    break;
                }
                case ValidationException validationException:
                {
                    response = new
                    {
                        validationException.Message
                    };
                    statusCode = 400;
                    break;
                }
                case UnauthorizedAccessException unauthorizedAccessException:
                {
                    statusCode = 401;
                    break;
                }
                case ForbiddenException forbiddenException:
                {
                    statusCode = 403;
                    break;
                }
                default:
                    response = GetDefaultResponse(context);
                    statusCode = 500;
                    break;
            }

            var contextResult = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
            context.Result = contextResult;

            switch (context.Exception)
            {
                case OutOfMemoryException ex:
                    _logger.Fatal(ex, "Out of memory exception");
                    break;
                case ValidationException ex: // оставляем только сообщение, т.к стак трейс не интересен
                    _logger.Warn(ex.Message);
                    break;
                default:
                    _logger.Error(context.Exception, context.Exception.Message);
                    break;
            }
        }

        public object GetDefaultResponse(ExceptionContext context)
        {
            return new
            {
                Message = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                    ? "Что-то пошло не так"
                    : context.Exception.ToString()
            };
        }
    }
}