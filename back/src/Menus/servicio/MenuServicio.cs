using Menus.entidad;
using Menus.repositorio;

namespace Menus.servicio;

public interface IMenuServicio
{
    Task<int> CrearMenuAsync(Menu menu);
    Task<Menu> ObtenerMenuAsync(int id);
    Task<List<Menu>> ObtenerMenusAsync();
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
}
