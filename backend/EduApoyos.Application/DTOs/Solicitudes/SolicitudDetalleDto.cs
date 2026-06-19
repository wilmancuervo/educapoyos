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
}

public class HistorialEstadoDto
{
    public string EstadoAnterior { get; set; } = string.Empty;
    public string EstadoNuevo { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; }
}
