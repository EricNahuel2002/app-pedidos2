using ApiGateway.Test.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Test;


public class GatewayE2ETests
{
    private readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5100")
    };

    private string CrearTokenDePrueba(int idUsuario, string rol)
    {
        var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
        new Claim("role", rol)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FJ39dk20slA9sLq93KDlq02Lskf92KDl"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "auth-api",
            audience: "api-gateway",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [Fact]
    public async Task SiHayMenuElGatewayDebeReenviarPeticionALaApiCorrespondienteYRetornarJson()
    {
        var respuesta = await _client.GetAsync("/menus/1");

        respuesta.EnsureSuccessStatusCode();
        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        var menu = JsonSerializer.Deserialize<MenuDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(menu);

        Assert.Equal(1, menu.Id);
        Assert.False(string.IsNullOrWhiteSpace(menu.Nombre));
    }

    [Fact]
    public async Task AlMostrarMenusElGatewayDebeReenviarPeticionYRetornarJson()
    {
        var respuesta = await _client.GetAsync("/menus");

        Assert.True(respuesta.IsSuccessStatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact]
    public async Task AlCrearUMenuElGatewayDebeReenviarPeticionYRetornarJson()
    {
        int idUsuario = 1;

        var token = CrearTokenDePrueba(idUsuario, "administrador");

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        MenuDto menu = new MenuDto { Nombre = "Empanadas", Descripcion = "Ricas empanadas de jamon y queso", Precio = 10, Imagen = "nada" };
        var respuesta = await _client.PostAsJsonAsync("/menus/crear",menu);

        Assert.True(respuesta.IsSuccessStatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));
    }







    [Fact]
    public async Task AlCancelarUnaOrdenElGatewayDebeReenviarPeticionALaApiCorrespondienteYRetornarJson()
    {
        int idUsuario = 1;
        var token = CrearTokenDePrueba(idUsuario, "cliente");

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CancelarOrdenRequestDto dto = new CancelarOrdenRequestDto { IdOrden = 1};

        var respuesta = await _client.PatchAsJsonAsync("/ordenes/cancelar", dto);

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact]
    public async Task AlConfirmarUnaOrdenElGatewayDebeReenviarPeticionALaApiCorrespondienteYRetornarJson()
    {
        int idUsuario = 1;
        int idMenu = 1;
        var token = CrearTokenDePrueba(idUsuario, "cliente");

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var respuesta = await _client.GetAsync($"/ordenes/confirmarOrden/{idMenu}");

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact]
    public async Task AlListarOrdenesDelClienteElGatewayDebeReenviarPeticionALaApiCorrespondienteYRetornarJson()
    {
        int idUsuario = 1;
        var token = CrearTokenDePrueba(idUsuario, "cliente");

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var respuesta = await _client.GetAsync("/ordenes/cliente");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));

        Assert.Contains("mensaje", json);
    }









    [Fact]
    public async Task AlValidarLoginElGatewayDebeReenviarPeticionYRetornarJson()
    {
        var loginDto = new LoginDto("ericaquino2002@gmail.com", "123456");

        var respuesta = await _client.PostAsJsonAsync("/usuarios/validarLogin", loginDto);

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));

        Assert.Contains("data", json);
    }

    [Fact]
    public async Task AlListarClientePorIdElGatewayDebeReenviarPeticionYRetornarJson()
    {
        int idCliente = 1;

        var respuesta = await _client.GetAsync($"/usuarios/cliente/{idCliente}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));

        Assert.Contains("data", json);
    }



    [Fact]
    public async Task AlHacerLoginElGatewayDebeRetornarOkYConfigurarCookie()
    {
        var loginDto = new { Email = "ericaquino2002@gmail.com", Contrasenia = "123456" };

        var respuesta = await _client.PostAsJsonAsync("/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var json = await respuesta.Content.ReadAsStringAsync();
        Assert.Contains("rol", json);

        bool tieneCookie = respuesta.Headers.TryGetValues("Set-Cookie", out var cookies);
        Assert.True(tieneCookie);
        Assert.Contains(cookies, c => c.Contains("access_token"));
    }

    [Fact]
    public async Task AlVerificarSesionElGatewayDebeValidarTokenYRetornarDatosDelUsuario()
    {
        int idUsuario = 1;
        string email = "ericaquino2002@gmail.com";
        string rol = "cliente";

        var token = CrearTokenDePrueba(idUsuario, rol);

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var respuesta = await _client.GetAsync("/auth/haySesionValida");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.Contains("id", json);
        Assert.Contains("rol", json);
        Assert.Contains(idUsuario.ToString(), json);
    }
}
