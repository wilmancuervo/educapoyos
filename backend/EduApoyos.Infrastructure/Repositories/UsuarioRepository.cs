using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyos.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
}
