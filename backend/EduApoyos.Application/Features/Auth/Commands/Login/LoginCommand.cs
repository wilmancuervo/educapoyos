using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Domain.Common;
using MediatR;

namespace EduApoyos.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
