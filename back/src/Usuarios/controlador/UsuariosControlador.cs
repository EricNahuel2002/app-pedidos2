using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.dto;
using Usuarios.excepciones;
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

        [HttpPost("validarLogin")]
        public async Task<IActionResult> ValidarCredencialesDeUsuario(LoginDto dto)
        {
            try
            {
                var usuario = await _usuarioServicio.ValidarCredencialesDeUsuario(dto);
                return Ok(usuario);
            }
            catch (CredencialesInvalidasException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno");
            }
        }


        [HttpGet("cliente/{id}")]
        public async Task<IActionResult> ListarCliente(int id)
        {
            try
            {
                UsuarioClienteDto dto = await _usuarioServicio.ObtenerUsuarioCliente(id);
                return Ok(dto);
            }catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
