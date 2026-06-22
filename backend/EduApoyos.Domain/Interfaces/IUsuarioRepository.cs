using EduApoyos.Domain.Entities;

namespace EduApoyos.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<(IEnumerable<Usuario> Items, int Total)> GetEstudiantesPagedAsync(int page, int pageSize);
}
