using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class Estudiante
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; private set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public string ProgramaAcademico { get; set; } = string.Empty;
    public int Semestre { get; set; }

    public Usuario Usuario { get; private set; } = null!;
    public ICollection<SolicitudApoyo> Solicitudes { get; private set; } = [];
}
