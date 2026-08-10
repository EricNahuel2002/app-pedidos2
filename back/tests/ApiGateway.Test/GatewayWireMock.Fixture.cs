using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Nodes;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ApiGateway.Test;

public class GatewayWireMockFixture : IDisposable
{
    private const string ClaveJwtDev = "FJ39dk20slA9sLq93KDlq02Lskf92KDl";

    public WireMockServer Servidor { get; }
    public HttpClient Cliente { get; }

    private readonly WebApplicationFactory<Program> _fabrica;
    private readonly string _directorioOcelotTemporal;
    private readonly string _directorioActualOriginal;
    private readonly (string Clave, string? ValorOriginal)? _claveJwtOriginal;
    private readonly (string Clave, string? ValorOriginal)? _emisorJwtOriginal;
    private readonly (string Clave, string? ValorOriginal)? _audienciaJwtOriginal;

    public GatewayWireMockFixture()
    {
        Servidor = WireMockServer.Start();
        ConfigurarStubs();

        var directorioGateway = ObtenerDirectorioProyectoGateway();
        _directorioOcelotTemporal = Path.Combine(Path.GetTempPath(), "apigw-ocelot-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directorioOcelotTemporal);
        CopiarAppSettings(directorioGateway, _directorioOcelotTemporal);
        ReescribirOcelotsHaciaWireMock(directorioGateway, _directorioOcelotTemporal, Servidor.Port);

        _claveJwtOriginal = EstablecerVariableDeEntorno("Jwt__Key", ClaveJwtDev);
        _emisorJwtOriginal = EstablecerVariableDeEntorno("Jwt__Issuer", "auth-api");
        _audienciaJwtOriginal = EstablecerVariableDeEntorno("Jwt__Audience", "api-gateway");

        _directorioActualOriginal = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_directorioOcelotTemporal);

        _fabrica = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.UseEnvironment("Testing");
                b.UseContentRoot(_directorioOcelotTemporal);
            });

        Cliente = _fabrica.CreateClient();

        Directory.SetCurrentDirectory(_directorioActualOriginal);
    }

    public void Dispose()
    {
        Directory.SetCurrentDirectory(_directorioActualOriginal);
        RestaurarVariableDeEntorno(_claveJwtOriginal);
        RestaurarVariableDeEntorno(_emisorJwtOriginal);
        RestaurarVariableDeEntorno(_audienciaJwtOriginal);
        _fabrica?.Dispose();
        Servidor?.Dispose();
        if (_directorioOcelotTemporal != null && Directory.Exists(_directorioOcelotTemporal))
        {
            try
            {
                Directory.Delete(_directorioOcelotTemporal, recursive: true);
            }
            catch
            {
                // si algún archivo quedó bloqueado, el SO lo limpia solo
            }
        }
    }

    private void ConfigurarStubs()
    {
        Servidor.Given(Request.Create().WithPath("/api/menus").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200)
                .WithBodyAsJson(new[] { new { Id = 1, Nombre = "Empanadas", Precio = 10 } }));

        Servidor.Given(Request.Create().WithPath("/api/ordenes/cliente").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200)
                .WithBodyAsJson(new[] { new { idOrden = 1, estado = "Pendiente" } }));
    }

    private static string ObtenerDirectorioProyectoGateway()
    {
        var directorio = new DirectoryInfo(AppContext.BaseDirectory);
        while (directorio != null)
        {
            var proyecto = Path.Combine(directorio.FullName, "src", "ApiGateway", "ApiGateway.csproj");
            if (File.Exists(proyecto))
                return Path.Combine(directorio.FullName, "src", "ApiGateway");
            directorio = directorio.Parent;
        }
        throw new DirectoryNotFoundException("No se encontró el directorio del proyecto ApiGateway");
    }

    private static void CopiarAppSettings(string directorioGateway, string destino)
    {
        foreach (var archivo in Directory.GetFiles(directorioGateway, "appsettings*.json"))
            File.Copy(archivo, Path.Combine(destino, Path.GetFileName(archivo)), overwrite: true);
    }

    private static void ReescribirOcelotsHaciaWireMock(string directorioGateway, string destino, int puerto)
    {
        foreach (var archivo in Directory.GetFiles(directorioGateway, "ocelot*.json"))
        {
            var contenido = JsonNode.Parse(File.ReadAllText(archivo))!.AsObject();
            if (contenido["Routes"] is JsonArray rutas)
            {
                foreach (var ruta in rutas)
                {
                    if (ruta?["DownstreamHostAndPorts"] is JsonArray hostPuertos)
                    {
                        foreach (var hostPuerto in hostPuertos)
                        {
                            hostPuerto!["Host"] = "127.0.0.1";
                            hostPuerto["Port"] = puerto;
                        }
                    }
                }
            }
            File.WriteAllText(Path.Combine(destino, Path.GetFileName(archivo)), contenido.ToJsonString());
        }
    }

    private static (string Clave, string? ValorOriginal)? EstablecerVariableDeEntorno(string clave, string valor)
    {
        var original = Environment.GetEnvironmentVariable(clave);
        Environment.SetEnvironmentVariable(clave, valor);
        return (clave, original);
    }

    private static void RestaurarVariableDeEntorno((string Clave, string? ValorOriginal)? variable)
    {
        if (variable.HasValue)
            Environment.SetEnvironmentVariable(variable.Value.Clave, variable.Value.ValorOriginal);
    }
}
