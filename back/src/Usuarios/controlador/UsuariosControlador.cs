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

        [HttpPost("registrarCliente")]
        public async Task<IActionResult> RegistrarCliente(RegistrarClienteDto dto)
        {
            try
            {
                await _usuarioServicio.RegistrarClienteAsync(dto);
                return StatusCode(201, new { mensaje = "Cliente registrado correctamente" });
            }
            catch (EmailYaRegistradoException)
            {
                return Conflict(new { mensaje = "El email ya se encuentra registrado" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPost("registrarRepartidor")]
        public async Task<IActionResult> RegistrarRepartidor(RegistrarRepartidorDto dto)
        {
            try
            {
                await _usuarioServicio.RegistrarRepartidorAsync(dto);
                return StatusCode(201, new { mensaje = "Repartidor registrado correctamente" });
            }
            catch (EmailYaRegistradoException)
            {
                return Conflict(new { mensaje = "El email ya se encuentra registrado" });
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
