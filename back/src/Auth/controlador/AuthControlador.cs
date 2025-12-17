using Auth.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auth.controlador;

[Route("api/auth")]
[ApiController]
public class AuthControlador : ControllerBase
{

    private HttpClient _client;
    private readonly IConfiguration _configuration;

    public AuthControlador(IHttpClientFactory client, IConfiguration configuration)
    {
        _client = client.CreateClient("usuario");
        _configuration = configuration;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var respuesta = await _client.PostAsJsonAsync("/api/usuarios/validarLogin", dto);

        if (!respuesta.IsSuccessStatusCode)
        {
            return Unauthorized(new { mensaje = "Credenciales invalidas" });
        }

        var usuario = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();

        var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new List<Claim>
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
        new Claim("role", usuario.Rol),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        Response.Cookies.Append("access_token", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Path = "/"
        });


        return Ok(new
        {
            rol = usuario.Rol
        });

    }


    [Authorize]
    [HttpGet("haySesionValida")]
    public IActionResult VerificarSesionValida()
    {
        return Ok(new
        {
            id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
            rol = User.FindFirst("role")?.Value
        });
    }
}
