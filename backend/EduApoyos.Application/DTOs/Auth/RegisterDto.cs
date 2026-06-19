using EduApoyos.Domain.Enums;

namespace EduApoyos.Application.DTOs.Auth;

public class RegisterDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Rol Rol { get; set; }
}
