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
                var resultado = await _usuarioServicio.ValidarCredencialesDeUsuario(dto);
                return Ok(resultado);
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
                var resultado = await _usuarioServicio.ObtenerUsuarioCliente(id);
                return Ok(resultado);
            }catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("repartidor/{id}")]
        public async Task<IActionResult> ListarRepartidor(int id)
        {
            try
            {
                var resultado = await _usuarioServicio.ObtenerUsuarioRepartidor(id);
                return Ok(resultado);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
