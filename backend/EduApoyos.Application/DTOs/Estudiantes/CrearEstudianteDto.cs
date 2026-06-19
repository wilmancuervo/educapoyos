using EduApoyos.Domain.Enums;

namespace EduApoyos.Application.DTOs.Estudiantes;

public class CrearEstudianteDto
{
    public Guid UsuarioId { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public string ProgramaAcademico { get; set; } = string.Empty;
    public int Semestre { get; set; }
}
