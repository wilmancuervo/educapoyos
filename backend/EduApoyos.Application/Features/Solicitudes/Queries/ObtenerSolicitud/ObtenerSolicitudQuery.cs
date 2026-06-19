using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Enums;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ObtenerSolicitud;

public record ObtenerSolicitudQuery(Guid SolicitudId, Guid UsuarioId, Rol Rol) : IRequest<Result<SolicitudDetalleDto>>;
