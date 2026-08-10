using Menus.entidad;
using Menus.repositorio;

namespace Menus.servicio;

public interface IMenuServicio
{
    Task<int> CrearMenuAsync(Menu menu);
    Task<Menu> ObtenerMenuAsync(int id);
    Task<List<Menu>> ObtenerMenusAsync();
    Task<Menu> ActualizarMenuAsync(Menu menu);
    Task<bool> EliminarMenuAsync(int id);
}
public class MenuServicio : IMenuServicio
{
    private IMenuRepositorio _menuRepo;

    public MenuServicio(IMenuRepositorio menuRepo)
    {
        this._menuRepo = menuRepo;
    }

    public async Task<int> CrearMenuAsync(Menu menu)
    {
        return await _menuRepo.CrearMenuAsync(menu);
    }

    public async Task<Menu> ObtenerMenuAsync(int id)
    {
        return await _menuRepo.ObtenerMenuAsync(id);
    }

    public async Task<List<Menu>> ObtenerMenusAsync()
    {
        return await _menuRepo.ObtenerMenusAsync();
    }

    public async Task<Menu> ActualizarMenuAsync(Menu menu)
    {
        Menu existente = await _menuRepo.ObtenerMenuAsync(menu.Id);

        if (existente == null)
        {
            throw new KeyNotFoundException($"No se encontró el menú con id {menu.Id}");
        }

        existente.Nombre = menu.Nombre;
        existente.Descripcion = menu.Descripcion;
        existente.Precio = menu.Precio;
        existente.Imagen = menu.Imagen;

        return await _menuRepo.ActualizarMenuAsync(existente);
    }

    public async Task<bool> EliminarMenuAsync(int id)
    {
        Menu existente = await _menuRepo.ObtenerMenuAsync(id);

        if (existente == null)
        {
            throw new KeyNotFoundException($"No se encontró el menú con id {id}");
        }

        return await _menuRepo.EliminarMenuAsync(id);
    }
}
