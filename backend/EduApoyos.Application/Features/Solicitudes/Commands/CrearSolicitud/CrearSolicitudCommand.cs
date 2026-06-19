using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Enums;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.CrearSolicitud;

public record CrearSolicitudCommand(
    Guid UsuarioId,
    TipoApoyo TipoApoyo,
    decimal MontoSolicitado,
    string Descripcion) : IRequest<Result<SolicitudDto>>;
