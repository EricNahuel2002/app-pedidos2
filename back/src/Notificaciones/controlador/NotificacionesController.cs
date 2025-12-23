using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notificaciones.servicio;
using System.IdentityModel.Tokens.Jwt;

namespace Notificaciones.controlador;

[Route("api/notificaciones")]
[Authorize]
[ApiController]
public class NotificacionesController : ControllerBase
{

    private INotificacionesServicio _notificacionesServicio;

    public NotificacionesController(INotificacionesServicio servicio)
    {
        _notificacionesServicio = servicio;
    }


    
}
