namespace Usuarios.excepciones;

public class CredencialesInvalidasException:Exception
{
    public CredencialesInvalidasException():base("Credenciales invalidas")
    {
    }
}
