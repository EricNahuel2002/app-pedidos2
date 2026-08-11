namespace Ordenes.excepciones;

public class ResenaYaExisteException : Exception
{
    public ResenaYaExisteException() : base("Esta orden ya fue reseñada")
    {
    }
}
