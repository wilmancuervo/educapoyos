using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Enums;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudes;

public record ListarSolicitudesQuery(
    int Page,
    int PageSize,
    EstadoSolicitud? Estado = null,
    TipoApoyo? Tipo = null,
    DateTime? Desde = null,
    DateTime? Hasta = null) : IRequest<PagedResultDto<SolicitudDto>>;
