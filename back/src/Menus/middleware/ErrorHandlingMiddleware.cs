using System.Text.Json;

namespace Menus.middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Error no controlado en el microservicio Menus");
            await EscribirRespuestaAsync(context, ex);
        }
    }

    private static async Task EscribirRespuestaAsync(HttpContext context, Exception ex)
    {
        (int statusCode, string mensaje) = ex switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "El recurso solicitado no existe"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { mensaje }));
    }
}
