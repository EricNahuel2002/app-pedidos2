using Ordenes.dto;
using Ordenes.Entidad;
using Ordenes.excepciones;
using Ordenes.repositorio;

namespace Ordenes.servicio;

public interface IOrdenesServicio
{
    Task<string> CancelarOrdenDelCliente(int idCliente, int idOrden);
    Task<string> ConfirmarOrdenDelClienteAsync(int idCliente, int idMenu);
    Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario);
    Task<string> MarcarOrdenComoFinalizada(int idUsuario, int idOrden);
    Task<List<Orden>> ObtenerOrdenesPendientes();
    Task<string> TomarUnaOrden(int idUsuario, int idOrden);
    Task<List<Orden>> ObtenerOrdenesTomadasDelRepartidorAsync(int idUsuario);
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

        if (orden.Estado.Equals("CANCELADA"))
        {
            throw new OrdenYaCanceladaException();
        }


        if (orden.Estado.Equals("EN CURSO"))
        {
            throw new OrdenEnCursoException();
        }
        orden.Estado = "CANCELADA";

        await _repo.ActualizarEstadoDeOrden(orden);

        return "Orden cancelada";
    }

    public async Task<string> ConfirmarOrdenDelClienteAsync(int idCliente, int idMenu)
    {

        MenuDto? menu = await _http.GetFromJsonAsync<MenuDto>($"/menus/{idMenu}");

        if(menu == null)
        {
            throw new MenuInexistenteException();
        }

        ClienteDto? cliente = await _http.GetFromJsonAsync<ClienteDto>($"/usuarios/cliente/{idCliente}");

        if(cliente == null)
        {
            throw new UsuarioInexistenteException();
        }


        Orden orden = new Orden
        {
            IdCliente = cliente.Id,
            EmailCliente = cliente.Email,
            NombreCliente = cliente.Nombre,
            Direccion = cliente.Direccion,
            IdMenu = menu.Id,
            NombreMenu = menu.Nombre,
            PrecioAPagar = menu.Precio,
            Estado = "PENDIENTE",
            FechaOrden = DateTime.UtcNow
        };


        await _repo.GuardarOrdenDelClienteAsync(orden);
        return "Orden confirmada";

    }

    public async Task<List<Orden>> ObtenerOrdenesDelClienteAsync(int idUsuario)
    {
        return await _repo.ObtenerOrdenesDelClienteAsync(idUsuario);
    }



    public async Task<string> MarcarOrdenComoFinalizada(int idUsuario, int idOrden)
    {
        Orden orden = await _repo.ObtenerOrdenTomadaPorRepartidorAsync(idUsuario,idOrden);

        if(orden == null)
        {
            throw new KeyNotFoundException($"Orden con el idRepartidor: {idUsuario} e idOrden: {idOrden} no encontrada");
        }

        if(orden.Estado == "PENDIENTE" || orden.Estado == "FINALIZADA")
        {
            throw new InvalidOperationException("No se puede finalizar una orden pendiente o ya finalizada");
        }

        orden.Estado = "FINALIZADA";

        await _repo.ActualizarEstadoDeOrden(orden);

        return $"Orden finalizada";
    }

    public async Task<List<Orden>> ObtenerOrdenesPendientes()
    {
        return await _repo.ObtenerOrdenesPendientes();
    }

    public async Task<string> TomarUnaOrden(int idUsuario, int idOrden)
    {
        Orden orden = await _repo.ObtenerOrden(idOrden);

        if (orden == null)
        {
            throw new KeyNotFoundException($"Orden con idOrden: {idOrden} no encontrada");
        }

        RepartidorDto? repartidor = await _http.GetFromJsonAsync<RepartidorDto>($"usuarios/repartidor/{idUsuario}");

        if(repartidor == null)
        {
            throw new KeyNotFoundException($"Repartidor con idUsuario: {idUsuario} no encontrado");
        }

        if (orden.Estado != "PENDIENTE")
        {
            throw new InvalidOperationException("No se puede tomar una orden en curso o finalizada");
        }

        orden.IdRepartidor = repartidor.Id;
        orden.DniRepartidor = repartidor.Dni;
        orden.NombreRepartidor = repartidor.Nombre;
        orden.Estado = "EN CURSO";

        await _repo.ActualizarEstadoDeOrden(orden);

        return "Orden tomada exitosamente";
    }

    public async Task<List<Orden>> ObtenerOrdenesTomadasDelRepartidorAsync(int idUsuario)
    {
        return await _repo.ObtenerOrdenesTomadasDelRepartidorAsync(idUsuario);
    }
}
