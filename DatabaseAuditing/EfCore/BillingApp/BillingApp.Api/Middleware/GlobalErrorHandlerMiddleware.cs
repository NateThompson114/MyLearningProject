using System.Net;
using System.Text.Json;
using BillingApp.Core.Exceptions;

namespace BillingApp.Api.Middleware;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

    public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case CustomException e:
                    // Custom application error
                    response.StatusCode = e.StatusCode;
                    break;
                case KeyNotFoundException e:
                    // Not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // Unhandled error
                    _logger.LogError(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.InnerException?.Message ?? error?.Message });
            await response.WriteAsync(result);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class GlobalErrorHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
    }
}