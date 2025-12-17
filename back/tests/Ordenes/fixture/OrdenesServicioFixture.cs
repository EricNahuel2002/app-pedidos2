using Moq;
using Ordenes.repositorio;
using Ordenes.servicio;

public class OrdenesServicioFixture
{
    public Mock<IOrdenesRepositorio> repoMock;
    public Mock<IHttpClientFactory> factoryMock;
    public IOrdenesServicio ordenServicio;

    public OrdenesServicioFixture()
    {
        repoMock = new Mock<IOrdenesRepositorio>();
        factoryMock = new Mock<IHttpClientFactory>();

        var httpClient = new HttpClient();

        factoryMock.Setup(_ => _.CreateClient("Apigateway")).Returns(httpClient);

        ordenServicio = new OrdenesServicio(repoMock.Object, factoryMock.Object);
    }
}