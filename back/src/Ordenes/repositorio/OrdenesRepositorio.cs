using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.dto;
using Ordenes.Entidad;

namespace Ordenes.repositorio;

public interface IOrdenesRepositorio
{
    Task<int> ActualizarEstadoDeOrden(Orden orden);
    Task<int> GuardarOrdenDelClienteAsync(Orden orden);
    Task<Orden> ObtenerOrden(int idOrden);
    Task<Orden> ObtenerOrdenDelClienteAsync(int idCliente, int idOrden);
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario);
    Task<List<Orden>> ObtenerOrdenesPendientes();
    Task<List<Orden>> ObtenerOrdenesTomadasDelRepartidorAsync(int idUsuario);
    Task<Orden> ObtenerOrdenTomadaPorRepartidorAsync(int idUsuario, int idOrden);
}
public class OrdenesRepositorio : IOrdenesRepositorio
{
    private OrdenesDbContext _ctx;

    public OrdenesRepositorio(OrdenesDbContext ordenesDbContext)
    {
        _ctx = ordenesDbContext;
    }

    public async Task<int> ActualizarEstadoDeOrden(Orden orden)
    {
        _ctx.Ordenes.Update(orden);
        return await _ctx.SaveChangesAsync();
    }

    public async Task<int> GuardarOrdenDelClienteAsync(Orden orden)
    {
        _ctx.Ordenes.Add(orden);

        return await _ctx.SaveChangesAsync();
    }

    public async Task<Orden> ObtenerOrden(int idOrden)
    {
        return await _ctx.Ordenes.FirstOrDefaultAsync(o => o.IdOrden == idOrden);
    }

    public async Task<Orden> ObtenerOrdenDelClienteAsync(int idCliente, int idOrden)
    {
        return await _ctx.Ordenes.Where(o => o.IdCliente == idCliente && o.IdOrden == idOrden).FirstOrDefaultAsync();
    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario)
    {
        return await _ctx.Ordenes.Where(o => o.IdCliente == idUsuario).ToListAsync();
    }

    public async Task<List<Orden>> ObtenerOrdenesPendientes()
    {
        return await _ctx.Ordenes.Where(o => o.Estado == "PENDIENTE").ToListAsync();
    }

    public async Task<List<Orden>> ObtenerOrdenesTomadasDelRepartidorAsync(int idUsuario)
    {
        return await _ctx.Ordenes.Where(o => o.IdRepartidor == idUsuario).ToListAsync();
    }

    public async Task<Orden> ObtenerOrdenTomadaPorRepartidorAsync(int idUsuario, int idOrden)
    {
        return await _ctx.Ordenes.Where( o => o.IdOrden == idOrden && o.IdRepartidor == idUsuario).FirstOrDefaultAsync();
    }
}
