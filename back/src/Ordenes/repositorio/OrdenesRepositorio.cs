using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.dto;
using Ordenes.Entidad;

namespace Ordenes.repositorio;

public interface IOrdenesRepositorio
{
    Task<int> CancelarOrdenAsync(Orden orden);
    Task<int> GuardarOrdenDelClienteAsync(Orden orden);
    Task<Orden> ObtenerOrdenDelClienteAsync(int idCliente, int idOrden);
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario);
}
public class OrdenesRepositorio : IOrdenesRepositorio
{
    private OrdenesDbContext _ctx;

    public OrdenesRepositorio(OrdenesDbContext ordenesDbContext)
    {
        _ctx = ordenesDbContext;
    }

    public async Task<int> CancelarOrdenAsync(Orden orden)
    {

        _ctx.Ordenes.Update(orden);

        return await _ctx.SaveChangesAsync();
    }

    public async Task<int> GuardarOrdenDelClienteAsync(Orden orden)
    {
        _ctx.Ordenes.Add(orden);

        return await _ctx.SaveChangesAsync();
    }

    public async Task<Orden> ObtenerOrdenDelClienteAsync(int idCliente, int idOrden)
    {
        return await _ctx.Ordenes.Where(o => o.IdUsuario == idCliente && o.IdOrden == idOrden).FirstOrDefaultAsync();
    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario)
    {
        return await _ctx.Ordenes.Where(o => o.IdUsuario == idUsuario).ToListAsync();
    }
}
