using EduApoyos.API.Common;
using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Features.Auth.Commands.Login;
using EduApoyos.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Controllers;

/// <summary>
/// Autenticación y registro de usuarios.
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : AppController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Autentica un usuario y devuelve un token JWT.
    /// </summary>
    /// <param name="dto">Credenciales del usuario.</param>
    /// <returns>Token JWT y datos básicos del usuario.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resultado = await _mediator.Send(new LoginCommand(dto.Email, dto.Password));

        return Match(resultado, Ok, UnauthorizedError);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="dto">Datos del nuevo usuario.</param>
    /// <returns>Token JWT y datos básicos del usuario creado.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var resultado = await _mediator.Send(new RegisterCommand(dto.NombreCompleto, dto.Email, dto.Password, dto.Rol));

        return Match(resultado, Ok, ConflictError);
    }
}
