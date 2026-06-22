using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.DTOs.Estudiantes;

public class EstudianteDto
{
    public Guid UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? EstudianteId { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? TipoDocumento { get; set; }
    public string? ProgramaAcademico { get; set; }
    public int? Semestre { get; set; }

    public static EstudianteDto FromUsuario(Usuario usuario) => new()
    {
        UsuarioId = usuario.Id,
        NombreCompleto = usuario.NombreCompleto,
        Email = usuario.Email,
        EstudianteId = usuario.Estudiante?.Id,
        NumeroDocumento = usuario.Estudiante?.NumeroDocumento,
        TipoDocumento = usuario.Estudiante?.TipoDocumento.ToString(),
        ProgramaAcademico = usuario.Estudiante?.ProgramaAcademico,
        Semestre = usuario.Estudiante?.Semestre,
    };

    public static EstudianteDto FromEntity(Estudiante estudiante) => new()
    {
        UsuarioId = estudiante.UsuarioId,
        NombreCompleto = estudiante.Usuario.NombreCompleto,
        Email = estudiante.Usuario.Email,
        EstudianteId = estudiante.Id,
        NumeroDocumento = estudiante.NumeroDocumento,
        TipoDocumento = estudiante.TipoDocumento.ToString(),
        ProgramaAcademico = estudiante.ProgramaAcademico,
        Semestre = estudiante.Semestre,
    };
}
