using System.Net;
using EventsApp.BLL.Exceptions;
using EventsApp.Domain.Models;

namespace EventsApp.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            var (statusCode, errorMessage) = ex switch
            {
                InvalidOperationException operationEx => (HttpStatusCode.BadRequest, operationEx.Message),
                NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message),
                OperationCanceledException canceledEx => (HttpStatusCode.RequestTimeout, canceledEx.Message),
                UnauthorizedAccessException unauthorizedEx => (HttpStatusCode.Unauthorized, unauthorizedEx.Message),
                _ => (HttpStatusCode.InternalServerError, "Необработанная ошибка сервера")
            };
            
            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                ErrorMessage = errorMessage,
                Trace = ex.StackTrace
            };
            _logger.LogError(ex, "Произошла ошибка при работе приложения: {errorMessage}", errorMessage);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}