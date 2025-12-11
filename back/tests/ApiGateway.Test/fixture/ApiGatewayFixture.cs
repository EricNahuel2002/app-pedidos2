using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
namespace ApiGateway.Test.fixture;

public class ApiGatewayFixture: WebApplicationFactory<Program>
{
    public HttpClient Http { get; private set; }

    public ApiGatewayFixture()
    {
        Http = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureAppConfiguration(config =>
        {
            config.Sources.Clear();

            var environment = builder.GetSetting("environment") ?? "Development";

            config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            config.AddJsonFile("ocelot.ordenes.json", optional: false, reloadOnChange: true);

            config.AddJsonFile("ocelot.usuarios.json", optional: false, reloadOnChange: true);

            config.AddJsonFile("ocelot.menus.json", optional: false, reloadOnChange: true);

            config.AddJsonFile("ocelot.notificaciones.json", optional: false, reloadOnChange: true);

            config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            config.AddEnvironmentVariables();
        });


        base.ConfigureWebHost(builder);
    }
}
