using ApiGateway.Test.dto;
using System.Net.Http.Json;
using System.Text.Json;

namespace ApiGateway.Test;


public class GatewayE2ETests
{
    private readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5100")
    };


    [Fact]
    public async Task AlMostrarUnMenuElGatewayDebeReenviarPeticionYRetornarJson()
    {
        var respuesta = await _client.GetAsync("/menus/1");

        Assert.True(respuesta.IsSuccessStatusCode);
        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact]
    public async Task SiHayMenuElGatewayDebePedirloYRetornarloCorrectamente()
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
        MenuDto menu = new MenuDto { Nombre = "Empanadas", Descripcion = "Ricas empanadas de jamon y queso", Precio = 10, Imagen = "nada" };
        var respuesta = await _client.PostAsJsonAsync("/menus/crear",menu);

        Assert.True(respuesta.IsSuccessStatusCode);

        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var json = await respuesta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));
    }

        //Get_menus_should_forward_call


//        PostCrearMenuDebeReenviarAlDownstream
}
