

namespace ApiGateway.Test;


public class GatewayE2ETests
{
    private readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5000")
    };


    [Fact]
    public async Task SiHaceLaPeticionParaPedirUnMenuElGatewayDebeReenviarYRetornarJson()
    {
        var respuesta = _client.GetAsync("/menu/1");
    }

        //Get_menus_should_forward_call


//        PostCrearMenuDebeReenviarAlDownstream
}
