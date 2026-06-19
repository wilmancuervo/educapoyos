using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Interfaces;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
        if (usuario is null)
            return Result.Failure<AuthResponseDto>(ApplicationErrors.Auth.CredencialesInvalidas);

        if (!_passwordHasher.Verify(request.Password, usuario.PasswordHash))
            return Result.Failure<AuthResponseDto>(ApplicationErrors.Auth.CredencialesInvalidas);

        var token = _jwtService.GenerarToken(usuario);

        return Result.Success(AuthResponseDto.FromEntity(usuario, token));
    }
}
