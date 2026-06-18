using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Rol Rol { get; set; }
    public DateTime FechaRegistro { get; set; }

    public Estudiante? Estudiante { get; set; }
}
