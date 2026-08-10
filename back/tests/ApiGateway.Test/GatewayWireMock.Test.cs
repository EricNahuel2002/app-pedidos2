using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Test;

public class GatewayWireMockTests : IClassFixture<GatewayWireMockFixture>
{
    private const string ClaveJwtDev = "FJ39dk20slA9sLq93KDlq02Lskf92KDl";

    private readonly HttpClient _cliente;

    public GatewayWireMockTests(GatewayWireMockFixture fixture)
    {
        _cliente = fixture.Cliente;
    }

    [Fact]
    public async Task AlConsultarUnaRutaPublicaSinTokenElGatewayReenviaYRetornaElContenidoDelServicio()
    {
        _cliente.DefaultRequestHeaders.Authorization = null;

        var respuesta = await _cliente.GetAsync("/menus");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        Assert.Contains("Empanadas", cuerpo);
    }

    [Fact]
    public async Task AlConsultarUnaRutaProtegidaConTokenValidoElGatewayReenviaYRetornaElContenidoDelServicio()
    {
        _cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", CrearToken("1", "cliente"));

        var respuesta = await _cliente.GetAsync("/ordenes/cliente");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        Assert.Contains("Pendiente", cuerpo);
    }

    [Fact]
    public async Task AlConsultarUnaRutaProtegidaSinTokenElGatewayRetorna401()
    {
        _cliente.DefaultRequestHeaders.Authorization = null;

        var respuesta = await _cliente.GetAsync("/ordenes/cliente");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    [Fact]
    public async Task AlConsultarUnaRutaProtegidaConUnRolNoPermitidoElGatewayRetorna403()
    {
        _cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", CrearToken("2", "repartidor"));

        var respuesta = await _cliente.GetAsync("/ordenes/cliente");

        Assert.Equal(HttpStatusCode.Forbidden, respuesta.StatusCode);
    }

    private static string CrearToken(string idUsuario, string rol)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, idUsuario),
            new Claim("role", rol)
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClaveJwtDev));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "auth-api",
            audience: "api-gateway",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
