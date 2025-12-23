using Microsoft.EntityFrameworkCore;
using Usuarios.contexto;
using Usuarios.entidad;

namespace Usuarios.repositorio;

public interface IUsuariosRepositorio
{
    Task<Usuario> ObtenerUsuarioPorEmail(string email);
    Task<Usuario> ObtenerUsuarioPorId(int id);
}
public class UsuariosRepositorio: IUsuariosRepositorio
{
    private UsuariosDbContext _ctx;

    public UsuariosRepositorio(UsuariosDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Usuario> ObtenerUsuarioPorEmail(string email)
    {
        return await _ctx.Usuarios.Where(u => u.Email.Equals(email))
            .Include(u => u.Cliente)
            .Include(u => u.Repartidor)
            .Include(u => u.UsuarioRoles)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObtenerUsuarioPorId(int id)
    {
        return await _ctx.Usuarios.Where(u => u.Id == id)
            .Include(u => u.Repartidor)
            .Include(u => u.Cliente)
            .FirstOrDefaultAsync();
    }
}
