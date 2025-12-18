using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Test.dto;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Contrasenia { get; set; } = null!;

    public LoginDto(string email, string contrasenia)
    {
        Email = email;
        Contrasenia = contrasenia;
    }
}
