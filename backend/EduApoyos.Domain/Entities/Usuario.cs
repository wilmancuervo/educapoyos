using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Rol Rol { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    public Estudiante? Estudiante { get; private set; }

    public void CambiarPassword(string nuevoHash)
    {
        PasswordHash = nuevoHash;
    }

    protected Usuario() { }

    public Usuario(string nombreCompleto, string email, string passwordHash, Rol rol)
    {
        Id = Guid.NewGuid();
        NombreCompleto = nombreCompleto;
        Email = email;
        PasswordHash = passwordHash;
        Rol = rol;
        FechaRegistro = DateTime.UtcNow;
    }
}
