using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyos.Infrastructure.Repositories;

public class SolicitudApoyoRepository : Repository<SolicitudApoyo>, ISolicitudApoyoRepository
{
    public SolicitudApoyoRepository(AppDbContext context) : base(context) { }

    public async Task<SolicitudApoyo?> GetByIdWithHistorialAsync(Guid id) =>
        await _dbSet
            .Include(s => s.Estudiante).ThenInclude(e => e.Usuario)
            .Include(s => s.Asesor)
            .Include(s => s.Historial).ThenInclude(h => h.Usuario)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<SolicitudApoyo>> GetByEstudianteIdAsync(Guid estudianteId) =>
        await _dbSet
            .Where(s => s.EstudianteId == estudianteId)
            .Include(s => s.Estudiante).ThenInclude(e => e.Usuario)
            .Include(s => s.Asesor)
            .Include(s => s.Historial)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();

    public async Task AddHistorialAsync(HistorialEstado historial) =>
        await _context.Set<HistorialEstado>().AddAsync(historial);

    public async Task<(IEnumerable<SolicitudApoyo> Items, int Total)> GetPagedAsync(
        int page,
        int pageSize,
        EstadoSolicitud? estado = null,
        TipoApoyo? tipo = null,
        DateTime? desde = null,
        DateTime? hasta = null)
    {
        var query = _dbSet
            .Include(s => s.Estudiante).ThenInclude(e => e.Usuario)
            .Include(s => s.Asesor)
            .AsQueryable();

        if (estado.HasValue)
            query = query.Where(s => s.Estado == estado.Value);

        if (tipo.HasValue)
            query = query.Where(s => s.TipoApoyo == tipo.Value);

        if (desde.HasValue)
            query = query.Where(s => s.FechaSolicitud >= desde.Value);

        if (hasta.HasValue)
            query = query.Where(s => s.FechaSolicitud <= hasta.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.FechaSolicitud)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}
