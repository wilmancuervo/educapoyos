using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Interfaces;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existente = await _usuarioRepository.GetByEmailAsync(request.Email);
        if (existente is not null)
            return Result.Failure<AuthResponseDto>(ApplicationErrors.Auth.EmailYaRegistrado);

        var hash = _passwordHasher.Hash(request.Password);
        var usuario = new Usuario(request.NombreCompleto, request.Email, hash, request.Rol);

        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        var token = _jwtService.GenerarToken(usuario);

        return Result.Success(AuthResponseDto.FromEntity(usuario, token));
    }
}
