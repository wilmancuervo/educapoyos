using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;

    public static AuthResponseDto FromEntity(Usuario usuario, string token) => new()
    {
        Token = token,
        NombreCompleto = usuario.NombreCompleto,
        Email = usuario.Email,
        Rol = usuario.Rol.ToString()
    };
}
