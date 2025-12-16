using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.Entidad;

namespace Ordenes.repositorio;

public interface IOrdenesRepositorio
{
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario);
}
public class OrdenesRepositorio : IOrdenesRepositorio
{
    private OrdenesDbContext _ctx;

    public OrdenesRepositorio(OrdenesDbContext ordenesDbContext)
    {
        _ctx = ordenesDbContext;
    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario)
    {
        return await _ctx.Ordenes.Where(o => o.IdUsuario == idUsuario).ToListAsync();
    }
}
