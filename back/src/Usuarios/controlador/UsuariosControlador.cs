using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.dto;
using Usuarios.servicio;

namespace Usuarios.controlador
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosControlador : ControllerBase
    {
        private IUsuariosServicio _usuarioServicio;

        public UsuariosControlador(IUsuariosServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public async Task<IActionResult> ValidarCredencialesDeUsuario(LoginDto dto)
        {
            try
            {
                var usuario = await _usuarioServicio.ValidarCredencialesDeUsuario(dto);
                return Ok(usuario);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
