using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyos.Infrastructure.Repositories;

public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
{
    public EstudianteRepository(AppDbContext context) : base(context) { }

    public async Task<Estudiante?> GetByUsuarioIdAsync(Guid usuarioId) =>
        await _dbSet.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

    public async Task<(IEnumerable<Estudiante> Items, int Total)> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbSet.Include(e => e.Usuario).AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}
