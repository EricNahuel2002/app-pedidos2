using Microsoft.EntityFrameworkCore;
using Usuarios.contexto;
using Usuarios.entidad;

namespace Usuarios.repositorio;

public interface IUsuariosRepositorio
{
    Task<Usuario> ObtenerUsuarioPorEmail(string email);
    Task<Usuario> ObtenerUsuarioPorId(int id);
    Task<Rol> ObtenerRolPorNombre(string nombre);
    Task<Usuario> GuardarUsuarioAsync(Usuario usuario);
    Task<List<Usuario>> ObtenerTodosLosUsuariosAsync();
    Task<List<Usuario>> ObtenerRepartidoresPendientesAsync();
    Task<bool> VerificarRepartidorAsync(int id);
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

    public async Task<Rol> ObtenerRolPorNombre(string nombre)
    {
        return await _ctx.Roles.Where(r => r.Nombre.Equals(nombre)).FirstOrDefaultAsync();
    }

    public async Task<Usuario> GuardarUsuarioAsync(Usuario usuario)
    {
        _ctx.Usuarios.Add(usuario);
        await _ctx.SaveChangesAsync();
        return usuario;
    }

    public async Task<List<Usuario>> ObtenerTodosLosUsuariosAsync()
    {
        return await _ctx.Usuarios
            .Include(u => u.Cliente)
            .Include(u => u.Repartidor)
            .Include(u => u.UsuarioRoles)
            .ThenInclude(ur => ur.Rol)
            .ToListAsync();
    }

    public async Task<List<Usuario>> ObtenerRepartidoresPendientesAsync()
    {
        return await _ctx.Usuarios
            .Where(u => u.Repartidor != null && !u.Repartidor.Verificado)
            .Include(u => u.Repartidor)
            .ToListAsync();
    }

    public async Task<bool> VerificarRepartidorAsync(int id)
    {
        Repartidor? repartidor = await _ctx.Repartidores.FirstOrDefaultAsync(r => r.IdUsuario == id);

        if (repartidor == null)
        {
            return false;
        }

        repartidor.Verificado = true;
        await _ctx.SaveChangesAsync();
        return true;
    }
}
