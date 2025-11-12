using System.Net;
using System.Text.Json;

namespace ExpenseManagement.WebAPI.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var message = "An error occurred processing your request";

        switch (exception)
        {
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                message = "Unauthorized access";
                break;
            case ArgumentException:
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
        }

        var result = JsonSerializer.Serialize(new { message, statusCode = (int)code });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
