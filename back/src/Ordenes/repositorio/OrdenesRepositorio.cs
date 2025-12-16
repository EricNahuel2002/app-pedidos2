using Ordenes.contexto;

namespace Ordenes.repositorio;

public interface IOrdenesRepositorio
{

}
public class OrdenesRepositorio : IOrdenesRepositorio
{
    private OrdenesDbContext ordenesDbContext;

    public OrdenesRepositorio(OrdenesDbContext ordenesDbContext)
    {
        this.ordenesDbContext = ordenesDbContext;
    }
}
