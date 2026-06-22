using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyos.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<(IEnumerable<Usuario> Items, int Total)> GetEstudiantesPagedAsync(int page, int pageSize)
    {
        var query = _dbSet
            .Where(u => u.Rol == Rol.Estudiante)
            .Include(u => u.Estudiante)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.NombreCompleto)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}
