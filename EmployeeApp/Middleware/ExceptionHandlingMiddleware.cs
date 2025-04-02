using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace EmployeeApp.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); // Попытка выполнить следующую часть конвейера
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex); // Если ошибка, вызываем обработку
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        _logger.LogError(exception, "An unhandled exception occurred.");

        var response = new
        {
            message = exception.Message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}