namespace EduApoyos.Application.DTOs.Solicitudes;

public class AsignarAsesorDto
{
    public Guid SolicitudId { get; set; }
    public Guid AsesorId { get; set; }
    public string Observacion { get; set; } = string.Empty;
}
