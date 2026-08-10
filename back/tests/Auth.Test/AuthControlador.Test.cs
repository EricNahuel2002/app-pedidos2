using Auth.controlador;
using Auth.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;

namespace Auth.Test;

public class AuthControladorTest
{
    private const string ClaveJwt = "FJ39dk20slA9sLq93KDlq02Lskf92KDl";

    private static AuthControlador CrearControlador(HttpResponseMessage respuestaDeUsuarios, out DefaultHttpContext contexto)
    {
        var handler = new FakeHttpMessageHandler(respuestaDeUsuarios);
        var cliente = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var fabrica = new Mock<IHttpClientFactory>();
        fabrica.Setup(f => f.CreateClient("usuario")).Returns(cliente);

        var configuracion = new Mock<IConfiguration>();
        configuracion.Setup(c => c["Jwt:Key"]).Returns(ClaveJwt);
        configuracion.Setup(c => c["Jwt:Issuer"]).Returns("auth-api");
        configuracion.Setup(c => c["Jwt:Audience"]).Returns("api-gateway");
        configuracion.Setup(c => c["Jwt:ExpireMinutes"]).Returns("30");

        contexto = new DefaultHttpContext();

        return new AuthControlador(fabrica.Object, configuracion.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = contexto }
        };
    }

    [Fact]
    public async Task AlLoginExitosaRetornaOkConElRolYConfiguraLaCookieDeAcceso()
    {
        var respuestaDeUsuarios = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { Id = 1, Email = "pepe@gmail.com", Rol = "cliente" })
        };
        var controlador = CrearControlador(respuestaDeUsuarios, out var contexto);

        var resultado = await controlador.Login(new LoginDto { Email = "pepe@gmail.com", Contrasenia = "123456" });

        var ok = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(200, ok.StatusCode);

        var json = System.Text.Json.JsonSerializer.Serialize(ok.Value);
        Assert.Contains("cliente", json);

        Assert.True(contexto.Response.Headers.ContainsKey("Set-Cookie"));
        var cookies = contexto.Response.Headers["Set-Cookie"];
        Assert.True(cookies.Count > 0);
        var cookie = cookies.First(c => c.StartsWith("access_token="));
        Assert.Contains("httponly", cookie, StringComparison.OrdinalIgnoreCase);

        var token = cookie.Substring("access_token=".Length).Split(';')[0];
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.Contains(jwt.Claims, c => c.Type == "sub" && c.Value == "1");
        Assert.Contains(jwt.Claims, c => c.Type == "role" && c.Value == "cliente");
        Assert.Contains(jwt.Claims, c => c.Type == "email" && c.Value == "pepe@gmail.com");
    }

    [Fact]
    public async Task SiLasCredencialesSonInvalidasElLoginRetorna401YNoConfiguraLaCookie()
    {
        var respuestaDeUsuarios = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var controlador = CrearControlador(respuestaDeUsuarios, out var contexto);

        var resultado = await controlador.Login(new LoginDto { Email = "pepe@gmail.com", Contrasenia = "incorrecta" });

        var noAutorizado = Assert.IsType<UnauthorizedObjectResult>(resultado);
        Assert.Equal(401, noAutorizado.StatusCode);
        Assert.False(contexto.Response.Headers.ContainsKey("Set-Cookie"));
    }

    [Fact]
    public void AlVerificarSesionRetornaLosDatosDelUsuarioAutenticado()
    {
        var controlador = CrearControlador(new HttpResponseMessage(), out var contexto);

        var claims = new[]
        {
            new Claim("sub", "1"),
            new Claim("email", "pepe@gmail.com"),
            new Claim("role", "cliente")
        };
        contexto.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "prueba"));

        var resultado = controlador.VerificarSesionValida();

        var ok = Assert.IsType<OkObjectResult>(resultado);
        var json = System.Text.Json.JsonSerializer.Serialize(ok.Value);
        Assert.Contains("\"id\":\"1\"", json);
        Assert.Contains("\"email\":\"pepe@gmail.com\"", json);
        Assert.Contains("\"rol\":\"cliente\"", json);
    }

    [Fact]
    public void LaVerificacionDeSesionRequiereAutorizacion()
    {
        var metodo = typeof(AuthControlador).GetMethod(nameof(AuthControlador.VerificarSesionValida));

        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<AuthorizeAttribute>());
    }

    [Fact]
    public void AlLogoutSeEliminaLaCookieDeAcceso()
    {
        var controlador = CrearControlador(new HttpResponseMessage(), out var contexto);

        var resultado = controlador.Logout();

        Assert.IsType<OkObjectResult>(resultado);
        Assert.True(contexto.Response.Headers.ContainsKey("Set-Cookie"));
        var cookies = contexto.Response.Headers["Set-Cookie"];
        Assert.True(cookies.Count > 0);
        var cookie = cookies.First(c => c.StartsWith("access_token="));
        Assert.Contains("expires=", cookie);
    }
}
