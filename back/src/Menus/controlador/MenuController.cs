using Menus.entidad;
using Menus.servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Menus.controlador;

[ApiController]
[Route("api/menus")]
public class MenuController : Controller
{
    
    private readonly IMenuServicio _menuServicio;
    public MenuController(IMenuServicio menuServicio)
    {
        _menuServicio = menuServicio;
    }
    [HttpPost("crear")]
    public async Task<IActionResult> CrearMenuAsync(Menu menu)
    {
        return Ok(await _menuServicio.CrearMenuAsync(menu));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuAsync(int id)
    {
        try
        {
            Menu menu = await _menuServicio.ObtenerMenuAsync(id);
            return Ok(menu);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error al obtener el menu");
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetMenusAsync()
    {
        try
        {
            return Ok(await _menuServicio.ObtenerMenusAsync());

        }
        catch (Exception)
        {
            return StatusCode(500, "Error al obtener los menus");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarMenuAsync(int id, Menu menu)
    {
        try
        {
            menu.Id = id;
            var actualizado = await _menuServicio.ActualizarMenuAsync(menu);
            return Ok(actualizado);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensaje = "El menú no existe" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error al actualizar el menu");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMenuAsync(int id)
    {
        try
        {
            await _menuServicio.EliminarMenuAsync(id);
            return Ok(new { mensaje = "Menú eliminado correctamente" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensaje = "El menú no existe" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error al eliminar el menu");
        }
    }
}
