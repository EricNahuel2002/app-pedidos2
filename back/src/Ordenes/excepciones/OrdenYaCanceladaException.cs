namespace Ordenes.excepciones;

public class OrdenYaCanceladaException:Exception
{
    public OrdenYaCanceladaException() : base("La orden ya fue cancelada")
    {

    }
}
