using EduApoyos.Domain.Common;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.CambiarEstado;

public record CambiarEstadoCommand(
    Guid SolicitudId,
    string Accion,
    Guid AsesorId,
    string Observacion) : IRequest<Result>;
