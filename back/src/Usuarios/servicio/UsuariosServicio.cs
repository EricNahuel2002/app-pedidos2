using Usuarios.dto;
using Usuarios.entidad;
using Usuarios.excepciones;
using Usuarios.repositorio;

namespace Usuarios.servicio;

public interface IUsuariosServicio
{
    Task<UsuarioDto> ValidarCredencialesDeUsuario(LoginDto dto);
}
public class UsuariosServicio: IUsuariosServicio
{

    private IUsuariosRepositorio _usuarioRepo;

    public UsuariosServicio(IUsuariosRepositorio repo)
    {
        _usuarioRepo = repo;
    }

    public async Task<UsuarioDto> ValidarCredencialesDeUsuario(LoginDto dto)
    {
        Usuario usuario = await _usuarioRepo.ObtenerUsuarioPorEmail(dto.Email);

        if(usuario == null || !dto.Contrasenia.Equals(usuario.Contrasenia))
        {
            throw new CredencialesInvalidasException();
        }

        Rol rol = usuario.UsuarioRoles.Select(ur => ur.Rol).FirstOrDefault() ?? throw new InvalidOperationException("El usuario no tiene roles asignados.");

        UsuarioDto usuarioDto = new UsuarioDto(usuario.Id, usuario.Email, rol.Nombre);

        return usuarioDto;
    }
}
