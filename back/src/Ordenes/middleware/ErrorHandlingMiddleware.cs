using System.Text.Json;
using Ordenes.excepciones;

namespace Ordenes.middleware;

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
            _logger.LogError(ex, "Error no controlado en el microservicio Ordenes");
            await EscribirRespuestaAsync(context, ex);
        }
    }

    private static async Task EscribirRespuestaAsync(HttpContext context, Exception ex)
    {
        (int statusCode, string mensaje) = ex switch
        {
            OrdenEnCursoException => (StatusCodes.Status400BadRequest, "La orden ya se encuentra en curso"),
            OrdenYaCanceladaException => (StatusCodes.Status400BadRequest, "La orden ya fue cancelada"),
            MenuInexistenteException => (StatusCodes.Status404NotFound, "El menú ingresado no existe"),
            UsuarioInexistenteException => (StatusCodes.Status404NotFound, "El usuario ingresado no existe"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "El recurso solicitado no existe"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { mensaje }));
    }
}
