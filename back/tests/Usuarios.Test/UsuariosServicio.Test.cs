using Moq;
using Usuarios.dto;
using Usuarios.entidad;
using Usuarios.excepciones;
using Usuarios.repositorio;
using Usuarios.servicio;
using Usuarios.Test.fixture;

namespace Usuarios.Test;

public class UsuariosServicioTest: IClassFixture<UsuariosServicioFixture>
{
    private Mock<IUsuariosRepositorio> _repoMock;
    private UsuariosServicio _usuarioServicio;

    public UsuariosServicioTest(UsuariosServicioFixture fixture)
    {
        _repoMock = fixture.repoMock;
        _usuarioServicio = fixture.usuarioServicio;
    }


    [Fact]
    public async Task QueSePuedaIniciarSesion()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "123");

        Usuario usuario = new Usuario
        {
            Id = 1, Email = dto.Email, Contrasenia = dto.Contrasenia,  Nombre = "pepe"
        };

        Cliente cliente = new Cliente
        {
            IdUsuario = usuario.Id, Direccion = "Lamadrid", NumeroTelefonico = "1234567891", Saldo = 90, Usuario = usuario
        };

        Rol rol = new Rol { Id = 1, Nombre = "cliente" };

        UsuarioRol usuarioRol = new UsuarioRol
        {
            Id = 1, IdUsuario = usuario.Id, Usuario = usuario, IdRol = rol.Id, Rol = rol
        };

        usuario.Cliente = cliente;

        usuario.UsuarioRoles.Add(usuarioRol);

        rol.UsuarioRoles.Add(usuarioRol);

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        UsuarioDto resultado = await _usuarioServicio.ValidarCredencialesDeUsuario(dto);

        Assert.Equal(dto.Email,resultado.Email);
    }

    [Fact]
    public async Task SiAlIniciarSesionLasCredencialesSonIncorrectasRetornaCredencialesInvalidasException()
    {
        LoginDto dto = new LoginDto("pepe@gmail.com", "contraseniacualquiera");

        Usuario usuario = new Usuario
        {
            Id = 1,
            Email = dto.Email,
            Contrasenia = "123",
            Nombre = "pepe"
        };

        Cliente cliente = new Cliente
        {
            IdUsuario = usuario.Id,
            Direccion = "Lamadrid",
            NumeroTelefonico = "1234567891",
            Saldo = 90,
            Usuario = usuario
        };

        Rol rol = new Rol { Id = 1, Nombre = "cliente" };

        UsuarioRol usuarioRol = new UsuarioRol
        {
            Id = 1,
            IdUsuario = usuario.Id,
            Usuario = usuario,
            IdRol = rol.Id,
            Rol = rol
        };

        usuario.Cliente = cliente;

        usuario.UsuarioRoles.Add(usuarioRol);

        rol.UsuarioRoles.Add(usuarioRol);

        _repoMock.Setup(r => r.ObtenerUsuarioPorEmail(dto.Email)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<CredencialesInvalidasException>(async () => await _usuarioServicio.ValidarCredencialesDeUsuario(dto));

    }

}
