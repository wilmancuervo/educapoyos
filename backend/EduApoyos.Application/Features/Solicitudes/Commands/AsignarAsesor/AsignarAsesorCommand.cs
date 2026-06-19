using EduApoyos.Domain.Common;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;

public record AsignarAsesorCommand(
    Guid SolicitudId,
    Guid AsesorId,
    Guid UsuarioQueAsignaId,
    string Observacion) : IRequest<Result>;
