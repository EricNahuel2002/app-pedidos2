
using Notificaciones.dto;

namespace Notificaciones.servicio;

public interface INotificacionesServicio
{
    Task<string> MarcarOrdenComoFinalizada(int idUsuario, int idOrden);
    Task<List<OrdenDto>> ObtenerOrdenesPendientes();
    Task<string> TomarUnaOrden(int idUsuario, int idOrden);
}
public class NotificacionesServicio : INotificacionesServicio
{

    private HttpClient _client;

    public NotificacionesServicio(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("Apigateway");
    }

    public async Task<string> MarcarOrdenComoFinalizada(int idUsuario, int idOrden)
    {
        var resultado = await _client.PatchAsJsonAsync("/ordenes/actualizarEstadoDeOrden",new ActualizarEstadoDeOrdenDto(idUsuario,idOrden,"Finalizada"));

        resultado.EnsureSuccessStatusCode();

        return $"Orden con id:{idOrden} finalizada";
    }

    public async Task<List<OrdenDto>> ObtenerOrdenesPendientes()
    {
        var respuesta = await _client.GetAsync("/ordenes");

        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<List<OrdenDto>>() ?? new List<OrdenDto>();
    }

    public async Task<string> TomarUnaOrden(int idUsuario, int idOrden)
    {
        var resultado = await _client.PatchAsJsonAsync("/ordenes/actualizarEstadoDeOrden",new ActualizarEstadoDeOrdenDto(idUsuario, idOrden,"En curso"));

        if (!resultado.IsSuccessStatusCode)
        {
            return "Orden ya tomada por otro repartidor o ya finalizada";
        }

        return "Orden tomada exitosamente";
    }

}
