namespace Ordenes.excepciones;

public class UsuarioInexistenteException : Exception
{
    public UsuarioInexistenteException() : base("El usuario ingresado no existe") { }

}
