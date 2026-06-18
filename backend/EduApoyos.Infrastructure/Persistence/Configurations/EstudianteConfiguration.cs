using EduApoyos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduApoyos.Infrastructure.Persistence.Configurations;

public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.NumeroDocumento)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(e => e.NumeroDocumento)
            .IsUnique();

        builder.Property(e => e.TipoDocumento)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.ProgramaAcademico)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Semestre)
            .IsRequired();

        builder.HasOne(e => e.Usuario)
            .WithOne(u => u.Estudiante)
            .HasForeignKey<Estudiante>(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
