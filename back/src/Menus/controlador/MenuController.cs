using Menus.entidad;
using Menus.servicio;
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

    public async Task<IActionResult> CrearMenuAsync(Menu menu)
    {
        return Ok(await _menuServicio.CrearMenuAsync(menu));
    }

    public async Task<IActionResult> GetMenuAsync(int id)
    {
        try
        {
            Menu menu = await _menuServicio.ObtenerMenuAsync(id);
            return Ok(menu);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener el menu: " + ex.Message);
        }
    }

    public async Task<IActionResult> GetMenusAsync()
    {
        try
        {
            return Ok(await _menuServicio.ObtenerMenusAsync());

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener los menus: " + ex.Message);
        }
    }
}
