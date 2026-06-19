using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.DTOs.Solicitudes;

public class SolicitudDetalleDto
{
    public Guid Id { get; set; }
    public Guid EstudianteId { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public string EmailEstudiante { get; set; } = string.Empty;
    public string ProgramaAcademico { get; set; } = string.Empty;
    public int Semestre { get; set; }
    public string TipoApoyo { get; set; } = string.Empty;
    public decimal MontoSolicitado { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public Guid? AsesorId { get; set; }
    public string? NombreAsesor { get; set; }
    public IEnumerable<HistorialEstadoDto> Historial { get; set; } = [];

    public static SolicitudDetalleDto FromEntity(SolicitudApoyo solicitud) => new()
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
        Historial = solicitud.Historial.OrderBy(h => h.FechaCambio).Select(HistorialEstadoDto.FromEntity)
    };
}

public class HistorialEstadoDto
{
    public string EstadoAnterior { get; set; } = string.Empty;
    public string EstadoNuevo { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; }

    public static HistorialEstadoDto FromEntity(HistorialEstado historial) => new()
    {
        EstadoAnterior = historial.EstadoAnterior.ToString(),
        EstadoNuevo = historial.EstadoNuevo.ToString(),
        Observacion = historial.Observacion,
        NombreUsuario = historial.Usuario.NombreCompleto,
        FechaCambio = historial.FechaCambio
    };
}
