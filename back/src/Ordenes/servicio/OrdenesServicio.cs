using Ordenes.Entidad;
using Ordenes.dto;
using Ordenes.repositorio;

namespace Ordenes.servicio;

public interface IOrdenesServicio
{
    Task<string> CancelarOrdenDelCliente(int idCliente, int idOrden);
    Task<string> ConfirmarOrdenDelCliente(ClienteMenuDto dto);
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idCliente);
}
public class OrdenesServicio : IOrdenesServicio
{
    private IOrdenesRepositorio _repo;

    public OrdenesServicio(IOrdenesRepositorio repo)
    {
        this._repo = repo;
    }


    public async Task<string> CancelarOrdenDelCliente(int idCliente, int idOrden)
    {
        return "Orden cancelada";
    }

    public async Task<string> ConfirmarOrdenDelCliente(ClienteMenuDto dto)
    {
        return "Orden confirmada";
    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idCliente)
    {
        throw new NotImplementedException();
    }
}
