using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.repositorio;
using Usuarios.servicio;

namespace Usuarios.Test.fixture;

public class UsuariosServicioFixture
{
    public Mock<IUsuariosRepositorio> repoMock;
    public UsuariosServicio usuarioServicio;

    public UsuariosServicioFixture()
    {
        repoMock = new Mock<IUsuariosRepositorio>();
        usuarioServicio = new UsuariosServicio(repoMock.Object);
    }
}
