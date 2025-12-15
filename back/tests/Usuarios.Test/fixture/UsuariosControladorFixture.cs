using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.controlador;
using Usuarios.servicio;

namespace Usuarios.Test.fixture;

public class UsuariosControladorFixture
{
    public Mock<IUsuariosServicio> servicioMock;
    public UsuariosControlador controlador;

    public UsuariosControladorFixture()
    {
        servicioMock = new Mock<IUsuariosServicio>();
        controlador = new UsuariosControlador(servicioMock.Object);
    }
}
