using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;

public record ListarSolicitudesEstudianteQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<SolicitudDto>>>;
