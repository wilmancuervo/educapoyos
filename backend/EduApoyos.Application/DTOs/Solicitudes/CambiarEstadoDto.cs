namespace EduApoyos.Application.DTOs.Solicitudes;

public class CambiarEstadoDto
{
    public Guid SolicitudId { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}
