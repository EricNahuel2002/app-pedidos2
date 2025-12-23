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

    public OrdenesServicioFixture()
    {
        repoMock = new Mock<IOrdenesRepositorio>();
        factoryMock = new Mock<IHttpClientFactory>();

        // HttpMessageHandler falso que responde según la ruta
        var handler = new FakeHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://apigateway:5000/")
        };

        factoryMock.Setup(_ => _.CreateClient("Apigateway")).Returns(httpClient);

        ordenServicio = new OrdenesServicio(repoMock.Object, factoryMock.Object);
    }

    // Handler simple para pruebas: devuelve JSON para /menus/{id} y /usuarios/cliente/{id}
    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Rutas relativas: request.RequestUri.AbsolutePath contiene el path
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

            // Por defecto 404 para rutas no esperadas
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}