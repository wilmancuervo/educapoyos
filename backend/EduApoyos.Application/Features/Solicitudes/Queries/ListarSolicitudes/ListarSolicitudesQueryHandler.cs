using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudes;

public class ListarSolicitudesQueryHandler : IRequestHandler<ListarSolicitudesQuery, PagedResultDto<SolicitudDto>>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;

    public ListarSolicitudesQueryHandler(ISolicitudApoyoRepository solicitudRepository)
    {
        _solicitudRepository = solicitudRepository;
    }

    public async Task<PagedResultDto<SolicitudDto>> Handle(ListarSolicitudesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _solicitudRepository.GetPagedAsync(request.Page, request.PageSize, request.Estado, request.Tipo, request.Desde, request.Hasta);

        var dtos = items.Select(s => new SolicitudDto
        {
            Id = s.Id,
            EstudianteId = s.EstudianteId,
            NombreEstudiante = s.Estudiante.Usuario.NombreCompleto,
            TipoApoyo = s.TipoApoyo.ToString(),
            MontoSolicitado = s.MontoSolicitado,
            Descripcion = s.Descripcion,
            Estado = s.Estado.ToString(),
            FechaSolicitud = s.FechaSolicitud,
            FechaActualizacion = s.FechaActualizacion,
            NombreAsesor = s.Asesor?.NombreCompleto
        });

        return new PagedResultDto<SolicitudDto>
        {
            Items = dtos,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
