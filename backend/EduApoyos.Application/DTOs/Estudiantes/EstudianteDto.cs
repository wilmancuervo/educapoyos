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
}
