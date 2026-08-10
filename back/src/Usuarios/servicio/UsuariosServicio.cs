using BCryptHash = BCrypt.Net.BCrypt;
using Usuarios.dto;
using Usuarios.entidad;
using Usuarios.excepciones;
using Usuarios.repositorio;

namespace Usuarios.servicio;

public interface IUsuariosServicio
{
    Task<UsuarioClienteDto> ObtenerUsuarioCliente(int id);
    Task<UsuarioRepartidorDto> ObtenerUsuarioRepartidor(int id);
    Task<UsuarioDto> ValidarCredencialesDeUsuario(LoginDto dto);
    Task RegistrarClienteAsync(RegistrarClienteDto dto);
    Task RegistrarRepartidorAsync(RegistrarRepartidorDto dto);
}
public class UsuariosServicio : IUsuariosServicio
{

    private IUsuariosRepositorio _usuarioRepo;

    public UsuariosServicio(IUsuariosRepositorio repo)
    {
        _usuarioRepo = repo;
    }

    public async Task<UsuarioClienteDto> ObtenerUsuarioCliente(int id)
    {
        Usuario usuario = await _usuarioRepo.ObtenerUsuarioPorId(id);

        if (usuario == null)
            throw new KeyNotFoundException($"No se encontró el usuario con id {id}");
        if (usuario.Cliente == null)
            throw new InvalidOperationException($"El usuario {usuario.Id} no tiene un cliente asociado");

        UsuarioClienteDto dto = new UsuarioClienteDto()
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Direccion = usuario.Cliente.Direccion,
            NumeroTelefonico = usuario.Cliente.NumeroTelefonico,
            Saldo = usuario.Cliente.Saldo
        };

        return dto;

    }

    public async Task<UsuarioRepartidorDto> ObtenerUsuarioRepartidor(int id)
    {
        Usuario usuario = await _usuarioRepo.ObtenerUsuarioPorId(id);

        if (usuario == null)
            throw new KeyNotFoundException($"No se encontró el usuario con id {id}");
        if (usuario.Repartidor == null)
            throw new InvalidOperationException($"El usuario {usuario.Id} no tiene un repartidor asociado");

        UsuarioRepartidorDto dto = new UsuarioRepartidorDto()
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Dni = usuario.Repartidor.Dni,
            FotoDniUrl = usuario.Repartidor.FotoDniUrl,
            Verificado = usuario.Repartidor.Verificado
        };

        return dto;
    }

    public async Task<UsuarioDto> ValidarCredencialesDeUsuario(LoginDto dto)
    {
        Usuario usuario = await _usuarioRepo.ObtenerUsuarioPorEmail(dto.Email);

        if(usuario == null || !BCryptHash.Verify(dto.Contrasenia, usuario.Contrasenia))
        {
            throw new CredencialesInvalidasException();
        }
        if(usuario.UsuarioRoles == null)
        {
            throw new InvalidOperationException($"El usuario no tiene usuariosRoles asignados");
        }

        Rol rol = usuario.UsuarioRoles.Select(ur => ur.Rol).FirstOrDefault() ?? throw new InvalidOperationException("El usuario no tiene roles asignados.");

        UsuarioDto usuarioDto = new UsuarioDto(usuario.Id, usuario.Email, rol.Nombre);

        return usuarioDto;
    }

    public async Task RegistrarClienteAsync(RegistrarClienteDto dto)
    {
        await ValidarEmailDisponible(dto.Email);

        Usuario usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Contrasenia = BCryptHash.HashPassword(dto.Contrasenia)
        };

        usuario.Cliente = new Cliente
        {
            Direccion = dto.Direccion,
            NumeroTelefonico = dto.Telefono,
            Saldo = 0
        };

        await AsignarRolYGuardar(usuario, "cliente");
    }

    public async Task RegistrarRepartidorAsync(RegistrarRepartidorDto dto)
    {
        await ValidarEmailDisponible(dto.Email);

        Usuario usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Contrasenia = BCryptHash.HashPassword(dto.Contrasenia)
        };

        usuario.Repartidor = new Repartidor
        {
            Dni = dto.Dni,
            FotoDniUrl = string.Empty,
            Verificado = false
        };

        await AsignarRolYGuardar(usuario, "repartidor");
    }

    private async Task ValidarEmailDisponible(string email)
    {
        Usuario existente = await _usuarioRepo.ObtenerUsuarioPorEmail(email);

        if (existente != null)
        {
            throw new EmailYaRegistradoException();
        }
    }

    private async Task AsignarRolYGuardar(Usuario usuario, string nombreRol)
    {
        Rol rol = await _usuarioRepo.ObtenerRolPorNombre(nombreRol);

        if (rol == null)
        {
            throw new InvalidOperationException($"El rol {nombreRol} no existe");
        }

        usuario.UsuarioRoles.Add(new UsuarioRol
        {
            Rol = rol
        });

        await _usuarioRepo.GuardarUsuarioAsync(usuario);
    }
}
