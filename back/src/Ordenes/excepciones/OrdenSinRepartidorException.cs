namespace Ordenes.excepciones;

public class OrdenSinRepartidorException : Exception
{
    public OrdenSinRepartidorException() : base("La orden no tiene un repartidor asignado")
    {
    }
}
