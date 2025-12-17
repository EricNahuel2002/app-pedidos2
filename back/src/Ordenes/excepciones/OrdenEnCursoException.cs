namespace Ordenes.excepciones;

public class OrdenEnCursoException:Exception
{
    public OrdenEnCursoException():base("No se puede cancelar una orden en curso")
    {

    }
}
