using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordenes.dto;
using Ordenes.servicio;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ordenes.controller;
[ApiController]
[Authorize]
[Route("api/ordenes")]
public class OrdenesController : Controller
{
    private IOrdenesServicio _ordenServicio;

    public OrdenesController(IOrdenesServicio ordenServicio)
    {
        this._ordenServicio = ordenServicio;
    }

    [HttpPatch("cancelar")]
    public async Task<IActionResult> CancelarOrdenDelCliente([FromBody]CancelarOrdenRequestDto dto)
    {
        try
        {
            if(dto == null)
            {
                return NotFound();
            }
            var idCliente = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(idCliente == null)
            {
                return Unauthorized();
            }
            var resultado = await _ordenServicio.CancelarOrdenDelCliente(int.Parse(idCliente), dto.IdOrden);
            return Ok(new { mensaje = resultado});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("confirmarOrden/{idMenu}")]
    public async Task<IActionResult> ConfirmarOrdenDelClienteAsync(int idMenu)
    {
        try
        {
            var idUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(idUsuario == null)
            {
                return Unauthorized();
            }
            var resultado = await _ordenServicio.ConfirmarOrdenDelClienteAsync(int.Parse(idUsuario),idMenu);
            return StatusCode(201, new {menasaje = resultado});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("cliente")]
    public async Task<IActionResult> ListarOrdenesDelClienteAsync()
    {
        try
        {
            var id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (id == null) 
            { 
                return Unauthorized(); 
            }

            int idUsuario = int.Parse(id);
            
            var resultado = await _ordenServicio.ObtenerOrdenesDelClienteAsync(idUsuario);
            return Ok(resultado);
        }
        catch(Exception e)
        {
            return StatusCode(500,e.Message);
        }
    }


    [HttpGet("ordenesPendientes")]
    public async Task<IActionResult> ListarOrdenesPendientes()
    {
        try
        {
            var ordenesPendientes = await _ordenServicio.ObtenerOrdenesPendientes();
            return Ok(ordenesPendientes);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpGet("tomarOrden/{idOrden}")]
    public async Task<IActionResult> TomarUnaOrden(int idOrden)
    {
        try
        {

            var idUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            var resultado = await _ordenServicio.TomarUnaOrden(int.Parse(idUsuario), idOrden);
            return Ok(resultado);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpPatch("marcarOrdenFinalizada")]
    public async Task<IActionResult> MarcarOrdenComoFinalizada([FromBody] int idOrden)
    {
        try
        {

            var idUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            var resultado = await _ordenServicio.MarcarOrdenComoFinalizada(int.Parse(idUsuario), idOrden);
            return Ok(resultado);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpGet("repartidor")]
    public async Task<IActionResult> ListarOrdenesTomadasDelRepartidorAsync()
    {
        try
        {
            var id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (id == null)
            {
                return Unauthorized();
            }

            int idUsuario = int.Parse(id);

            var resultado = await _ordenServicio.ObtenerOrdenesTomadasDelRepartidorAsync(idUsuario);
            return Ok(new { mensaje = resultado });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
