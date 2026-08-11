using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.Entidad;

namespace Ordenes.repositorio;

public interface IResenasRepositorio
{
    Task<Resena> GuardarResenaAsync(Resena resena);
    Task<Resena?> ObtenerResenaPorOrdenAsync(int idOrden);
    Task<Resena?> ObtenerResenaPorIdAsync(int id);
    Task<List<Resena>> ObtenerResenasDeRepartidorAsync(int idRepartidor);
    Task<List<Resena>> ObtenerTodasAsync();
    Task<bool> EliminarResenaAsync(int id);
    Task<string?> ObtenerNombreRepartidorAsync(int idRepartidor);
}
public class ResenasRepositorio : IResenasRepositorio
{
    private OrdenesDbContext _ctx;

    public ResenasRepositorio(OrdenesDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Resena> GuardarResenaAsync(Resena resena)
    {
        _ctx.Resenas.Add(resena);
        await _ctx.SaveChangesAsync();
        return resena;
    }

    public async Task<Resena?> ObtenerResenaPorOrdenAsync(int idOrden)
    {
        return await _ctx.Resenas.FirstOrDefaultAsync(r => r.IdOrden == idOrden);
    }

    public async Task<Resena?> ObtenerResenaPorIdAsync(int id)
    {
        return await _ctx.Resenas.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Resena>> ObtenerResenasDeRepartidorAsync(int idRepartidor)
    {
        return await _ctx.Resenas
            .Where(r => r.IdRepartidor == idRepartidor)
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();
    }

    public async Task<List<Resena>> ObtenerTodasAsync()
    {
        return await _ctx.Resenas
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();
    }

    public async Task<bool> EliminarResenaAsync(int id)
    {
        Resena? resena = await _ctx.Resenas.FirstOrDefaultAsync(r => r.Id == id);

        if (resena == null)
        {
            return false;
        }

        _ctx.Resenas.Remove(resena);
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<string?> ObtenerNombreRepartidorAsync(int idRepartidor)
    {
        return await _ctx.Ordenes
            .Where(o => o.IdRepartidor == idRepartidor && o.NombreRepartidor != null)
            .Select(o => o.NombreRepartidor)
            .FirstOrDefaultAsync();
    }
}
