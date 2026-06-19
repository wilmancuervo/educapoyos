using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Features.Auth.Commands.Login;
using EduApoyos.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resultado = await _mediator.Send(new LoginCommand(dto.Email, dto.Password));

        if (resultado.IsFailure)
            return Unauthorized(new { resultado.Error.Code, resultado.Error.Message });

        return Ok(resultado.Value);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var resultado = await _mediator.Send(new RegisterCommand(dto.NombreCompleto, dto.Email, dto.Password, dto.Rol));

        if (resultado.IsFailure)
            return Conflict(new { resultado.Error.Code, resultado.Error.Message });

        return Ok(resultado.Value);
    }
}
