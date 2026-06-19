using EduApoyos.Application.Interfaces;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EduApoyos.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();
        var hasher = services.GetRequiredService<IPasswordHasher>();

        if (await db.Usuarios.AnyAsync()) return;

        var asesor = new Usuario(
            nombreCompleto: "Admin Asesor",
            email: "asesor@educapoyos.com",
            passwordHash: hasher.Hash("Asesor@2025"),
            rol: Rol.Asesor);

        var usuarioEstudiante = new Usuario(
            nombreCompleto: "Estudiante Demo",
            email: "estudiante@educapoyos.com",
            passwordHash: hasher.Hash("Estudiante@2025"),
            rol: Rol.Estudiante);

        await db.Usuarios.AddRangeAsync(asesor, usuarioEstudiante);
        await db.SaveChangesAsync();

        var estudiante = new Estudiante(
            usuarioId: usuarioEstudiante.Id,
            numeroDocumento: "1012345678",
            tipoDocumento: TipoDocumento.CedulaCiudadania,
            programaAcademico: "Ingenieria de Sistemas",
            semestre: 5);

        await db.Estudiantes.AddAsync(estudiante);
        await db.SaveChangesAsync();

        var solicitud = new SolicitudApoyo(
            estudianteId: estudiante.Id,
            tipoApoyo: TipoApoyo.Beca,
            monto: 1500000,
            descripcion: "Solicitud de beca por mérito académico — semestre 2025-1.");

        await db.SolicitudesApoyo.AddAsync(solicitud);
        await db.SaveChangesAsync();
    }
}
