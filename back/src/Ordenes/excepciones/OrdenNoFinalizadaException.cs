namespace Ordenes.excepciones;

public class OrdenNoFinalizadaException : Exception
{
    public OrdenNoFinalizadaException() : base("Solo se pueden reseñar órdenes finalizadas")
    {
    }
}
