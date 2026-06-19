using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;

public class ListarSolicitudesEstudianteQueryHandler : IRequestHandler<ListarSolicitudesEstudianteQuery, Result<IEnumerable<SolicitudDto>>>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public ListarSolicitudesEstudianteQueryHandler(ISolicitudApoyoRepository solicitudRepository, IEstudianteRepository estudianteRepository)
    {
        _solicitudRepository = solicitudRepository;
        _estudianteRepository = estudianteRepository;
    }

    public async Task<Result<IEnumerable<SolicitudDto>>> Handle(ListarSolicitudesEstudianteQuery request, CancellationToken cancellationToken)
    {
        var estudiante = await _estudianteRepository.GetByUsuarioIdAsync(request.UsuarioId);
        if (estudiante is null)
            return Result.Failure<IEnumerable<SolicitudDto>>(ApplicationErrors.Estudiante.NoEncontrado);

        var solicitudes = await _solicitudRepository.GetByEstudianteIdAsync(estudiante.Id);

        var dtos = solicitudes.Select(s => new SolicitudDto
        {
            Id = s.Id,
            EstudianteId = s.EstudianteId,
            NombreEstudiante = estudiante.Usuario.NombreCompleto,
            TipoApoyo = s.TipoApoyo.ToString(),
            MontoSolicitado = s.MontoSolicitado,
            Descripcion = s.Descripcion,
            Estado = s.Estado.ToString(),
            FechaSolicitud = s.FechaSolicitud,
            FechaActualizacion = s.FechaActualizacion,
            NombreAsesor = s.Asesor?.NombreCompleto
        });

        return Result.Success(dtos);
    }
}
