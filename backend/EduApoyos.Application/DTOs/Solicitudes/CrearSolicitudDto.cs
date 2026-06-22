using EduApoyos.Domain.Enums;

namespace EduApoyos.Application.DTOs.Solicitudes;

public class CrearSolicitudDto
{
    public Guid? EstudianteUsuarioId { get; set; }
    public TipoApoyo TipoApoyo { get; set; }
    public decimal MontoSolicitado { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
