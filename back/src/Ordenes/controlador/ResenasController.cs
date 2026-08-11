using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordenes.dto;
using Ordenes.excepciones;
using Ordenes.servicio;
using System.IdentityModel.Tokens.Jwt;

namespace Ordenes.controller;

[ApiController]
[Authorize]
[Route("api/resenas")]
public class ResenasController : ControllerBase
{
    private IResenasServicio _resenaServicio;

    public ResenasController(IResenasServicio resenaServicio)
    {
        _resenaServicio = resenaServicio;
    }

    [HttpPost]
    public async Task<IActionResult> CrearResena([FromBody] CrearResenaDto dto)
    {
        try
        {
            var idCliente = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (idCliente == null)
            {
                return Unauthorized();
            }

            await _resenaServicio.CrearResena(int.Parse(idCliente), dto);
            return StatusCode(201, new { mensaje = "Reseña creada correctamente" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensaje = "La orden no existe" });
        }
        catch (OrdenNoFinalizadaException)
        {
            return BadRequest(new { mensaje = "Solo se pueden reseñar órdenes finalizadas" });
        }
        catch (OrdenSinRepartidorException)
        {
            return BadRequest(new { mensaje = "La orden no tiene un repartidor asignado" });
        }
        catch (ResenaYaExisteException)
        {
            return Conflict(new { mensaje = "Esta orden ya fue reseñada" });
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { mensaje = "El puntaje debe estar entre 1 y 5" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("mias")]
    public async Task<IActionResult> ObtenerResenasMias()
    {
        try
        {
            var idRepartidor = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (idRepartidor == null)
            {
                return Unauthorized();
            }

            var resultado = await _resenaServicio.ObtenerResenasMias(int.Parse(idRepartidor));
            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("administracion")]
    public async Task<IActionResult> ObtenerTodasParaAdministracion()
    {
        try
        {
            var resultado = await _resenaServicio.ObtenerTodasParaAdministracion();
            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("administracion/{id}")]
    public async Task<IActionResult> EliminarResena(int id)
    {
        try
        {
            await _resenaServicio.EliminarResena(id);
            return Ok(new { mensaje = "Reseña eliminada correctamente" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensaje = "La reseña no existe" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
