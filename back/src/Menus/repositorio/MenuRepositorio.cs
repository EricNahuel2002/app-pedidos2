using Menus.Context;
using Menus.entidad;
using Microsoft.EntityFrameworkCore;

namespace Menus.repositorio;

public interface IMenuRepositorio
{
    Task<int> CrearMenuAsync(Menu menu);
    Task<Menu> ObtenerMenuAsync(int idMenu);
    Task<List<Menu>> ObtenerMenusAsync();
    Task<Menu> ActualizarMenuAsync(Menu menu);
    Task<bool> EliminarMenuAsync(int id);
}
public class MenuRepositorio : IMenuRepositorio
{
    private MenuDbContext _context;

    public MenuRepositorio(MenuDbContext context)
    {
        _context = context;
    }

    public async Task<int> CrearMenuAsync(Menu menu)
    {
        _context.Menus.Add(menu);
        return await _context.SaveChangesAsync();
    }

    public async Task<Menu> ObtenerMenuAsync(int idMenu)
    {
        return await _context.Menus.FirstOrDefaultAsync(m => m.Id == idMenu);
    }

    public async Task<List<Menu>> ObtenerMenusAsync()
    {
        return await _context.Menus.ToListAsync();
    }

    public async Task<Menu> ActualizarMenuAsync(Menu menu)
    {
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();
        return menu;
    }

    public async Task<bool> EliminarMenuAsync(int id)
    {
        Menu? menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == id);

        if (menu == null)
        {
            return false;
        }

        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();
        return true;
    }
}
