using Microsoft.AspNetCore.Mvc;
using Ordenes.dto;
using Ordenes.servicio;

namespace Ordenes.controller;
[ApiController]
[Route("api/ordenes")]
public class OrdenesController : Controller
{
    private IOrdenesServicio _ordenServicio;

    public OrdenesController(IOrdenesServicio ordenServicio)
    {
        this._ordenServicio = ordenServicio;
    }

    public async Task<IActionResult> CancelarOrdenDelCliente(int idCliente, int idOrden)
    {
        try
        {
            var resultado = await _ordenServicio.CancelarOrdenDelCliente(idCliente, idOrden);
            return Ok(resultado);
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    public async Task<IActionResult> ConfirmarOrdenDelClienteAsync(ClienteMenuDto dto)
    {
        try
        {
            var resultado = await _ordenServicio.ConfirmarOrdenDelCliente(dto);
            return StatusCode(201, resultado);
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    public async Task<IActionResult> ListarOrdenesDelClienteAsync(int idCliente)
    {
        try
        {
            var resultado = await _ordenServicio.ObtenerOrdenesDelClienteAsync(idCliente);
            return Ok(resultado);
        }
        catch(Exception e)
        {
            return StatusCode(500,e.Message);
        }
    }
}
