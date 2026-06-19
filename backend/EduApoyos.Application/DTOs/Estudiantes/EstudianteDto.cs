using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.DTOs.Estudiantes;

public class EstudianteDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string ProgramaAcademico { get; set; } = string.Empty;
    public int Semestre { get; set; }

    public static EstudianteDto FromEntity(Estudiante estudiante) => new()
    {
        Id = estudiante.Id,
        UsuarioId = estudiante.UsuarioId,
        NombreCompleto = estudiante.Usuario.NombreCompleto,
        Email = estudiante.Usuario.Email,
        NumeroDocumento = estudiante.NumeroDocumento,
        TipoDocumento = estudiante.TipoDocumento.ToString(),
        ProgramaAcademico = estudiante.ProgramaAcademico,
        Semestre = estudiante.Semestre
    };
}
