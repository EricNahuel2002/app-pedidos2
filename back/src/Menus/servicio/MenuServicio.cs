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
        return 1;
    }

    public async Task<Menu> ObtenerMenuAsync(int id)
    {
        try
        {
            return await _menuRepo.ObtenerMenuAsync(id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Menu>> ObtenerMenusAsync()
    {
        try
        {
            return await _menuRepo.ObtenerMenusAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
