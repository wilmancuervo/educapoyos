using EduApoyos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduApoyos.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<SolicitudApoyo> SolicitudesApoyo => Set<SolicitudApoyo>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
