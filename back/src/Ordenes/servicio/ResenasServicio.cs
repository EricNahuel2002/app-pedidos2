using Ordenes.dto;
using Ordenes.Entidad;
using Ordenes.excepciones;
using Ordenes.repositorio;

namespace Ordenes.servicio;

public interface IResenasServicio
{
    Task CrearResena(int idCliente, CrearResenaDto dto);
    Task<ResenasRepartidorDto> ObtenerResenasMias(int idRepartidor);
    Task<List<ResenaDto>> ObtenerTodasParaAdministracion();
    Task EliminarResena(int id);
}
public class ResenasServicio : IResenasServicio
{
    private IResenasRepositorio _resenaRepo;
    private IOrdenesRepositorio _ordenRepo;

    public ResenasServicio(IResenasRepositorio resenaRepo, IOrdenesRepositorio ordenRepo)
    {
        _resenaRepo = resenaRepo;
        _ordenRepo = ordenRepo;
    }

    public async Task CrearResena(int idCliente, CrearResenaDto dto)
    {
        if (dto.Puntaje < 1 || dto.Puntaje > 5)
        {
            throw new InvalidOperationException("El puntaje debe estar entre 1 y 5");
        }

        Orden orden = await _ordenRepo.ObtenerOrdenDelClienteAsync(idCliente, dto.IdOrden);

        if (orden == null)
        {
            throw new KeyNotFoundException($"No se encontró la orden {dto.IdOrden}");
        }

        if (orden.Estado != "FINALIZADA")
        {
            throw new OrdenNoFinalizadaException();
        }

        if (orden.IdRepartidor == null)
        {
            throw new OrdenSinRepartidorException();
        }

        Resena existente = await _resenaRepo.ObtenerResenaPorOrdenAsync(dto.IdOrden);

        if (existente != null)
        {
            throw new ResenaYaExisteException();
        }

        Resena resena = new Resena
        {
            IdOrden = orden.IdOrden,
            IdCliente = orden.IdCliente,
            IdRepartidor = orden.IdRepartidor.Value,
            NombreCliente = orden.NombreCliente,
            NombreRepartidor = orden.NombreRepartidor ?? "",
            Puntaje = dto.Puntaje,
            Comentario = string.IsNullOrWhiteSpace(dto.Comentario) ? null : dto.Comentario,
            FechaCreacion = DateTime.UtcNow
        };

        await _resenaRepo.GuardarResenaAsync(resena);
    }

    public async Task<ResenasRepartidorDto> ObtenerResenasMias(int idRepartidor)
    {
        List<Resena> resenas = await _resenaRepo.ObtenerResenasDeRepartidorAsync(idRepartidor);

        string nombreRepartidor = resenas.FirstOrDefault()?.NombreRepartidor
            ?? await _resenaRepo.ObtenerNombreRepartidorAsync(idRepartidor)
            ?? "";

        return new ResenasRepartidorDto
        {
            IdRepartidor = idRepartidor,
            NombreRepartidor = nombreRepartidor,
            Promedio = resenas.Count == 0 ? 0 : Math.Round(resenas.Average(r => r.Puntaje), 1),
            Cantidad = resenas.Count,
            Resenas = resenas.Select(ToDto).ToList()
        };
    }

    public async Task<List<ResenaDto>> ObtenerTodasParaAdministracion()
    {
        List<Resena> resenas = await _resenaRepo.ObtenerTodasAsync();
        return resenas.Select(ToDto).ToList();
    }

    public async Task EliminarResena(int id)
    {
        Resena? resena = await _resenaRepo.ObtenerResenaPorIdAsync(id);

        if (resena == null)
        {
            throw new KeyNotFoundException($"No se encontró la reseña con id {id}");
        }

        await _resenaRepo.EliminarResenaAsync(id);
    }

    private static ResenaDto ToDto(Resena resena)
    {
        return new ResenaDto
        {
            Id = resena.Id,
            IdOrden = resena.IdOrden,
            IdCliente = resena.IdCliente,
            IdRepartidor = resena.IdRepartidor,
            NombreCliente = resena.NombreCliente,
            NombreRepartidor = resena.NombreRepartidor,
            Puntaje = resena.Puntaje,
            Comentario = resena.Comentario,
            FechaCreacion = resena.FechaCreacion
        };
    }
}
