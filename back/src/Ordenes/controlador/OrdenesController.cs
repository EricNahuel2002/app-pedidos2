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
            ClienteMenuDto dto = new ClienteMenuDto(int.Parse(idUsuario), idMenu);
            var resultado = await _ordenServicio.ConfirmarOrdenDelClienteAsync(dto);
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
            return Ok(new {mensaje = resultado });
        }
        catch(Exception e)
        {
            return StatusCode(500,e.Message);
        }
    }
}
