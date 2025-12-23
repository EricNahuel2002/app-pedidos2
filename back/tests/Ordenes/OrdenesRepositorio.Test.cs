using Microsoft.EntityFrameworkCore;
using Ordenes.contexto;
using Ordenes.Entidad;
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
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Orden orden = new Orden
            {
                IdOrden = 1,
                IdMenu = 1,
                IdCliente = 1,
                Estado = "Pendiente",
                Direccion = "saraza",
                EmailCliente = "saraza",
                NombreCliente = "saraza",
                NombreMenu = "saraza",
                PrecioAPagar = 5,
                FechaOrden = DateTime.Now
            };
            Orden orden2 = new Orden
            {
                IdOrden = 2,
                IdMenu = 2,
                IdCliente = 1,
                Estado = "Pendiente",
                Direccion = "saraza",
                EmailCliente = "saraza",
                NombreCliente = "saraza",
                NombreMenu = "saraza",
                PrecioAPagar = 5,
                FechaOrden = DateTime.Now
            };

            ctx.Ordenes.AddRange(orden,orden2);
            await ctx.SaveChangesAsync();
        }
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            var ordenes = await ctx.Ordenes.ToListAsync();

            Assert.NotEmpty(ordenes);
            Assert.Equal(2, ordenes.Count);
        }
    }

    [Fact]
    public async Task QueSePuedaGuardarLaOrdenDelCliente()
    {
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Orden orden = new Orden
            {
                IdOrden = 1,
                IdMenu = 1,
                IdCliente = 1,
                Estado = "Pendiente",
                Direccion = "saraza",
                EmailCliente = "saraza",
                NombreCliente = "saraza",
                NombreMenu = "saraza",
                PrecioAPagar = 5,
                FechaOrden = DateTime.Now
            };

            ctx.Ordenes.Add(orden);

            await ctx.SaveChangesAsync();
        }
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            var orden = await ctx.Ordenes.Where(o => o.IdOrden == 1).FirstOrDefaultAsync();

            Assert.NotNull(orden);
            Assert.IsType<Orden>(orden);
            Assert.Equal(1, orden.IdOrden);

        }
    }

    [Fact]
    public async Task QueSePuedaCancelarLaOrdenDelCliente()
    {
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Orden orden = new Orden { IdOrden = 1, IdMenu = 1, IdCliente = 1,
                Estado = "Pendiente" ,Direccion = "saraza", EmailCliente = "saraza",
                NombreCliente = "saraza", NombreMenu = "saraza", PrecioAPagar = 5, FechaOrden = DateTime.Now};

            ctx.Ordenes.Add(orden);

            await ctx.SaveChangesAsync();
        }
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            Orden orden = new Orden
            {
                IdOrden = 1,
                IdMenu = 1,
                IdCliente = 1,
                Estado = "Cancelada",
                Direccion = "saraza",
                EmailCliente = "saraza",
                NombreCliente = "saraza",
                NombreMenu = "saraza",
                PrecioAPagar = 5,
                FechaOrden = DateTime.Now
            };

            ctx.Ordenes.Update(orden);

            await ctx.SaveChangesAsync();
        }
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            Orden? orden = await ctx.Ordenes.Where(o => o.IdOrden == 1).FirstOrDefaultAsync();

            Assert.NotNull(orden);
            Assert.Equal("Cancelada", orden.Estado);
        }
    }

    [Fact]
    public async Task QueSePuedaObtenerUnaOrdenDelCliente()
    {
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            ctx.Database.EnsureCreated();
            ctx.Database.EnsureDeleted();

            Orden orden = new Orden
            {
                IdOrden = 1,
                IdMenu = 1,
                IdCliente = 1,
                Estado = "Cancelada",
                Direccion = "saraza",
                EmailCliente = "saraza",
                NombreCliente = "saraza",
                NombreMenu = "saraza",
                PrecioAPagar = 5,
                FechaOrden = DateTime.Now
            };

            ctx.Ordenes.Add(orden);
            await ctx.SaveChangesAsync();
        }
        using (var ctx = new OrdenesDbContext(_ctx))
        {
            Orden? orden = await ctx.Ordenes.Where(o => o.IdCliente == 1 && o.IdOrden == 1).FirstOrDefaultAsync();

            Assert.NotNull(orden);
            Assert.Equal(1, orden.IdOrden);
        }
    }
}
