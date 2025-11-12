namespace ExpenseManagement.WebAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        _logger.LogInformation("Handling request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        await _next(context);

        var elapsedTime = DateTime.UtcNow - startTime;

        _logger.LogInformation("Finished handling request: {Method} {Path} - Status {StatusCode} in {ElapsedMs}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsedTime.TotalMilliseconds);
    }
}
