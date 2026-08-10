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
            }catch(KeyNotFoundException)
            {
                return NotFound(new { mensaje = "El usuario no existe" });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { mensaje = "El usuario no tiene un perfil asociado" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
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
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "El usuario no existe" });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { mensaje = "El usuario no tiene un perfil asociado" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("administracion")]
        public async Task<IActionResult> ListarUsuariosParaAdministracion()
        {
            try
            {
                var resultado = await _usuarioServicio.ObtenerUsuariosParaAdministracion();
                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("administracion/repartidores/pendientes")]
        public async Task<IActionResult> ListarRepartidoresPendientes()
        {
            try
            {
                var resultado = await _usuarioServicio.ObtenerRepartidoresPendientes();
                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("administracion/repartidores/{id}/verificar")]
        public async Task<IActionResult> VerificarRepartidor(int id)
        {
            try
            {
                await _usuarioServicio.VerificarRepartidor(id);
                return Ok(new { mensaje = "Repartidor verificado correctamente" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "El repartidor no existe" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
