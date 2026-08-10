namespace Usuarios.excepciones;

public class EmailYaRegistradoException : Exception
{
    public EmailYaRegistradoException() : base("El email ya se encuentra registrado")
    {
    }
}
