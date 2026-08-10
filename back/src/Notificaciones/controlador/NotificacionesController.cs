using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notificaciones.dto;
using Notificaciones.servicio;
using System.IdentityModel.Tokens.Jwt;

namespace Notificaciones.controlador;

[Route("api/notificaciones")]
[Authorize]
[ApiController]
public class NotificacionesController : ControllerBase
{
    private INotificacionesServicio _notificacionesServicio;

    public NotificacionesController(INotificacionesServicio servicio)
    {
        _notificacionesServicio = servicio;
    }

    [HttpGet("usuario")]
    public async Task<IActionResult> ObtenerNotificacionesDelUsuario()
    {
        var idUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (idUsuario == null)
        {
            return Unauthorized();
        }

        var resultado = await _notificacionesServicio.ObtenerNotificacionesDelUsuarioAsync(int.Parse(idUsuario));
        return Ok(resultado);
    }

    [HttpPatch("{id}/leida")]
    public async Task<IActionResult> MarcarNotificacionComoLeida(int id)
    {
        var idUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (idUsuario == null)
        {
            return Unauthorized();
        }

        await _notificacionesServicio.MarcarNotificacionComoLeidaAsync(int.Parse(idUsuario), id);
        return Ok(new { mensaje = "Notificación marcada como leída" });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CrearNotificacion([FromBody] CrearNotificacionDto dto)
    {
        if (dto == null)
        {
            return BadRequest();
        }

        await _notificacionesServicio.CrearNotificacionAsync(dto.IdUsuario, dto.Mensaje);
        return StatusCode(201, new { mensaje = "Notificación creada" });
    }
}
