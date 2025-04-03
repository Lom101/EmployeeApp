using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace EmployeeApp.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext); // Попытка выполнить следующую часть конвейера
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
        
        logger.LogError(exception, "An unhandled exception occurred.");

        var response = new
        {
            message = exception.Message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}