using Ordenes.Entidad;
using Ordenes.dto;
using Ordenes.repositorio;
using Ordenes.excepciones;

namespace Ordenes.servicio;

public interface IOrdenesServicio
{
    Task<string> CancelarOrdenDelCliente(int idCliente, int idOrden);
    Task<string> ConfirmarOrdenDelClienteAsync(ClienteMenuDto dto);
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idCliente);
}
public class OrdenesServicio : IOrdenesServicio
{
    private IOrdenesRepositorio _repo;
    private HttpClient _http;

    public OrdenesServicio(IOrdenesRepositorio repo,IHttpClientFactory factory)
    {
        _repo = repo;
        _http = factory.CreateClient("Apigateway");
    }


    public async Task<string> CancelarOrdenDelCliente(int idCliente, int idOrden)
    {

        Orden orden = await _repo.ObtenerOrdenDelClienteAsync(idCliente, idOrden);

        if (orden.Estado.Equals("Cancelada"))
        {
            throw new OrdenYaCanceladaException();
        }


        if (orden.Estado.Equals("En curso"))
        {
            throw new OrdenEnCursoException();
        }
        orden.Estado = "Cancelada";

        await _repo.CancelarOrdenAsync(orden);

        return "Orden cancelada";
    }

    public async Task<string> ConfirmarOrdenDelClienteAsync(ClienteMenuDto dto)
    {

        MenuDto? menu = await _http.GetFromJsonAsync<MenuDto>($"/menus/{dto.idMenu}");

        if(menu == null)
        {
            throw new MenuInexistenteException();
        }

        ClienteDto? cliente = await _http.GetFromJsonAsync<ClienteDto>($"/usuarios/cliente/{dto.idUsuario}");

        if(cliente == null)
        {
            throw new UsuarioInexistenteException();
        }


        Orden orden = new Orden
        {
            IdUsuario = cliente.Id,
            EmailCliente = cliente.Email,
            NombreCliente = cliente.Nombre,
            Direccion = cliente.Direccion,
            IdMenu = menu.Id,
            NombreMenu = menu.Nombre,
            PrecioAPagar = menu.Precio,
            Estado = "Pendiente"
        };


        await _repo.GuardarOrdenDelClienteAsync(orden);
        return "Orden confirmada";

    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idCliente)
    {
        return await _repo.ObtenerOrdenesDelClienteAsync(idCliente);
    }
}
