using EduApoyos.Domain.Entities;

namespace EduApoyos.Application.Interfaces;

public interface IJwtService
{
    string GenerarToken(Usuario usuario);
}
