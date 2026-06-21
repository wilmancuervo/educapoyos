using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public static AuthResponseDto FromEntity(Usuario usuario, string token) => new()
    {
        Token = token,
    };
}
