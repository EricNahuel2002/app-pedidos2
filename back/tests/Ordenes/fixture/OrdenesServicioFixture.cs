using Moq;
using Ordenes.repositorio;
using Ordenes.servicio;
using Ordenes.dto;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class OrdenesServicioFixture
{
    public Mock<IOrdenesRepositorio> repoMock;
    public Mock<IHttpClientFactory> factoryMock;
    public IOrdenesServicio ordenServicio;
    public List<string> notificacionesEnviadas;

    public OrdenesServicioFixture()
    {
        notificacionesEnviadas = new List<string>();
        repoMock = new Mock<IOrdenesRepositorio>();
        factoryMock = new Mock<IHttpClientFactory>();

        // HttpMessageHandler falso que responde según la ruta y captura las notificaciones enviadas
        var handler = new FakeHttpMessageHandler(notificacionesEnviadas);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://apigateway:5000/")
        };

        var notificacionesClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://notificaciones:5000/")
        };

        factoryMock.Setup(_ => _.CreateClient("Apigateway")).Returns(httpClient);
        factoryMock.Setup(_ => _.CreateClient("Notificaciones")).Returns(notificacionesClient);

        ordenServicio = new OrdenesServicio(repoMock.Object, factoryMock.Object);
    }

    // Handler simple para pruebas: devuelve JSON para /menus/{id}, /usuarios/cliente/{id},
    // /usuarios/repartidor/{id} y acepta la creación de notificaciones.
    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private List<string> _notificaciones;

        public FakeHttpMessageHandler(List<string> notificaciones)
        {
            _notificaciones = notificaciones;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post && request.RequestUri.AbsolutePath.StartsWith("/api/notificaciones"))
            {
                var cuerpo = request.Content != null ? request.Content.ReadAsStringAsync().GetAwaiter().GetResult() : "";
                _notificaciones.Add(cuerpo);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Created));
            }

            if (request.RequestUri.AbsolutePath.StartsWith("/menus/"))
            {
                var menu = new MenuDto { Id = 1, Nombre = "Menu 1", Precio = 50 };
                var json = JsonSerializer.Serialize(menu);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });
            }

            if (request.RequestUri.AbsolutePath.StartsWith("/usuarios/cliente/"))
            {
                var cliente = new ClienteDto { Id = 1, Nombre = "Eric", Email = "ericaquino2002@gmail.com", Direccion = "Lamadrid" };
                var json = JsonSerializer.Serialize(cliente);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });
            }

            if (request.RequestUri.AbsolutePath.StartsWith("/usuarios/repartidor/"))
            {
                var repartidor = new RepartidorDto { Id = 1, Nombre = "Carlos", Dni = "12345678" };
                var json = JsonSerializer.Serialize(repartidor);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });
            }

            // Por defecto 404 para rutas no esperadas
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
