using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Common.Errors;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ObtenerSolicitud;

public class ObtenerSolicitudQueryHandler : IRequestHandler<ObtenerSolicitudQuery, Result<SolicitudDetalleDto>>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public ObtenerSolicitudQueryHandler(ISolicitudApoyoRepository solicitudRepository, IEstudianteRepository estudianteRepository)
    {
        _solicitudRepository = solicitudRepository;
        _estudianteRepository = estudianteRepository;
    }

    public async Task<Result<SolicitudDetalleDto>> Handle(ObtenerSolicitudQuery request, CancellationToken cancellationToken)
    {
        var solicitud = await _solicitudRepository.GetByIdWithHistorialAsync(request.SolicitudId);
        if (solicitud is null)
            return Result.Failure<SolicitudDetalleDto>(ApplicationErrors.Solicitud.NoEncontrada);

        if (request.Rol == Rol.Estudiante)
        {
            var estudiante = await _estudianteRepository.GetByUsuarioIdAsync(request.UsuarioId);
            if (estudiante is null || !solicitud.PerteneceA(estudiante.Id))
                return Result.Failure<SolicitudDetalleDto>(DomainErrors.Solicitud.AccesoDenegado);
        }

        var dto = new SolicitudDetalleDto
        {
            Id = solicitud.Id,
            EstudianteId = solicitud.EstudianteId,
            NombreEstudiante = solicitud.Estudiante.Usuario.NombreCompleto,
            EmailEstudiante = solicitud.Estudiante.Usuario.Email,
            ProgramaAcademico = solicitud.Estudiante.ProgramaAcademico,
            Semestre = solicitud.Estudiante.Semestre,
            TipoApoyo = solicitud.TipoApoyo.ToString(),
            MontoSolicitado = solicitud.MontoSolicitado,
            Descripcion = solicitud.Descripcion,
            Estado = solicitud.Estado.ToString(),
            FechaSolicitud = solicitud.FechaSolicitud,
            FechaActualizacion = solicitud.FechaActualizacion,
            AsesorId = solicitud.AsesorId,
            NombreAsesor = solicitud.Asesor?.NombreCompleto,
            Historial = solicitud.Historial.OrderBy(h => h.FechaCambio).Select(h => new HistorialEstadoDto
            {
                EstadoAnterior = h.EstadoAnterior.ToString(),
                EstadoNuevo = h.EstadoNuevo.ToString(),
                Observacion = h.Observacion,
                NombreUsuario = h.Usuario.NombreCompleto,
                FechaCambio = h.FechaCambio
            })
        };

        return Result.Success(dto);
    }
}
