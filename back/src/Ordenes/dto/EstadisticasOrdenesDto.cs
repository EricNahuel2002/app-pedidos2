namespace Ordenes.dto;

public class EstadisticasOrdenesDto
{
    public int Total { get; set; }
    public int Pendientes { get; set; }
    public int EnCurso { get; set; }
    public int Finalizadas { get; set; }
    public int Canceladas { get; set; }
}
