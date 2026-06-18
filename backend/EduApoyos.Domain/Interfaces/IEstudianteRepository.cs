using EduApoyos.Domain.Entities;

namespace EduApoyos.Domain.Interfaces;

public interface IEstudianteRepository : IRepository<Estudiante>
{
    Task<Estudiante?> GetByUsuarioIdAsync(Guid usuarioId);
    Task<(IEnumerable<Estudiante> Items, int Total)> GetPagedAsync(int page, int pageSize);
}
