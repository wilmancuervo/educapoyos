using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class Estudiante
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public string ProgramaAcademico { get; set; } = string.Empty;
    public int Semestre { get; set; }

    public Usuario Usuario { get; private set; } = null!;
    public ICollection<SolicitudApoyo> Solicitudes { get; private set; } = [];

    protected Estudiante() { }

    public Estudiante(Guid usuarioId, string numeroDocumento, TipoDocumento tipoDocumento, string programaAcademico, int semestre)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        NumeroDocumento = numeroDocumento;
        TipoDocumento = tipoDocumento;
        ProgramaAcademico = programaAcademico;
        Semestre = semestre;
    }
}
