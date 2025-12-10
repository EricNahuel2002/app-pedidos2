using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using Usuarios.repositorio;
using Usuarios.servicios;
namespace Usuarios.Tests;

public class UsuarioTest
{
    private readonly Mock<IUsuarioRepositorio> _repoMock;
    private readonly UsuarioServicio _usuarioService;

    [Fact]
    public void Test1()
    {
    }
}
