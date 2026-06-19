using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Enums;
using MediatR;

namespace EduApoyos.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string NombreCompleto, string Email, string Password, Rol Rol) : IRequest<Result<AuthResponseDto>>;
