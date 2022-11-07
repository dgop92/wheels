using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using SharedKernel.Errors;

namespace Wheels.WebApi.Middlewares;


public static class ExceptionMiddleware
{

    // TODO: fix using string enums
    public static HttpStatusCode GetStatusCodeFromErrorCode(string errorCode)
    {
        switch (errorCode)
        {
            case "INVALID_INPUT":
            case "INVALID_ID":
            case "INVALID_OPERATION":
            case "ID_NOT_PROVIDED":
                return HttpStatusCode.BadRequest;
            case "NOT_FOUND":
                return HttpStatusCode.NotFound;
            case "DUPLICATED_RECORD":
                return HttpStatusCode.Conflict;
            case "UNAUTHORIZED":
                return HttpStatusCode.Unauthorized;
            case "FORBIDDEN":
                return HttpStatusCode.Forbidden;
            case "APPLICATION_INTEGRITY_ERROR":
                return HttpStatusCode.InternalServerError;
            default:
                return HttpStatusCode.InternalServerError;
        }
    }

    public static async Task HandleException(HttpContext context)
    {
        string currentTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string path = context.Request.Path.ToString();

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>()!;
        var exception = exceptionHandlerPathFeature.Error;
        context.Response.ContentType = Application.Json;

        if (exception is BaseException)
        {
            BaseException baseException = (BaseException)exception;
            var errorCode = baseException.Code;
            var level = baseException.Level;
            var message = baseException.UserMessage;
            var errorParams = baseException.ErrorParams;
            var statusCode = GetStatusCodeFromErrorCode(errorCode);

            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new
                    {
                        timestamp = currentTimeStamp,
                        path = path,
                        statusCode = statusCode,
                        errorInfo = new
                        {
                            code = errorCode,
                            level = level,
                            message = message,
                            errorParams = errorParams
                        }
                    }
                )
            );
            return;
        }

        if (exception is BadHttpRequestException)
        {
            BadHttpRequestException httpException = (BadHttpRequestException)exception;

            context.Response.StatusCode = (int)httpException.StatusCode;
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new
                    {
                        timestamp = currentTimeStamp,
                        path = path,
                        statusCode = httpException.StatusCode,
                        message = httpException.Message
                    }
                )
            );
            return;
        }

        var internalErrorStatusCode = HttpStatusCode.InternalServerError;

        context.Response.StatusCode = (int)internalErrorStatusCode;
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                new
                {
                    timestamp = currentTimeStamp,
                    path = path,
                    statusCode = internalErrorStatusCode,
                    message = exception.Message
                }
            )
        );
    }
}