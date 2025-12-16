using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordenes.Test.fixture;

public class OrdenesRepositorioFixture
{
    public DbContextOptions<OrdenesDbContext> ctx;
    public IOrdenesRepositorio ordenesRepo;

    public OrdenesRepositorioFixture()
    {
        ctx = new DbContextOptionsBuilder<OrdenesDbContext>()
            .UseInMemoryDatabase(databaseName: "orden_db" + Guid.NewGuid().ToString())
            .Options;
        ordenesRepo = new OrdenesRepositorio(new OrdenesDbContext(ctx));
    }
}
