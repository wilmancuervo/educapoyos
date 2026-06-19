namespace EduApoyos.Application.DTOs.Solicitudes;

public class SolicitudDto
{
    public Guid Id { get; set; }
    public Guid EstudianteId { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public string TipoApoyo { get; set; } = string.Empty;
    public decimal MontoSolicitado { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string? NombreAsesor { get; set; }
}
