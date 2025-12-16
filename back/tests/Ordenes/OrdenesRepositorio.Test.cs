using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.repositorio;
using Ordenes.Test.fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordenes.Test;

public class OrdenesRepositorioTest: IClassFixture<OrdenesRepositorioFixture>
{
    private DbContextOptions<OrdenesDbContext> _ctx;
    private IOrdenesRepositorio repo;

    public OrdenesRepositorioTest(OrdenesRepositorioFixture fixture)
    {
        _ctx = fixture.ctx;
        repo = fixture.ordenesRepo;
    }


    [Fact]
    public async Task QueSePuedanObtenerLasOrdenesDelCliente()
    {

    }

    [Fact]
    public async Task QueSePuedaConfirmarLaOrdenDelCliente()
    {

    }

    [Fact]
    public async Task QueSePuedaCancelarLaOrdenDelCliente()
    {

    }
}
