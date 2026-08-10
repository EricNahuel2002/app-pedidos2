namespace Notificaciones.Test;

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _respuesta;

    public FakeHttpMessageHandler(HttpResponseMessage respuesta)
    {
        _respuesta = respuesta;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(_respuesta);
}
