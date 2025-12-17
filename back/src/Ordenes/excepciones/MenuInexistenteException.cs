namespace Ordenes.excepciones
{
    public class MenuInexistenteException:Exception
    {
        public MenuInexistenteException():base("El menu ingresado no existe")
        {

        }
    }
}
