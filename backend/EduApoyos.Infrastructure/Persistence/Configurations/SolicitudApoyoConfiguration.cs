using EduApoyos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduApoyos.Infrastructure.Persistence.Configurations;

public class SolicitudApoyoConfiguration : IEntityTypeConfiguration<SolicitudApoyo>
{
    public void Configure(EntityTypeBuilder<SolicitudApoyo> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TipoApoyo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.MontoSolicitado)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.Descripcion)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.FechaSolicitud)
            .IsRequired();

        builder.Property(s => s.FechaActualizacion)
            .IsRequired();

        builder.HasOne(s => s.Estudiante)
            .WithMany(e => e.Solicitudes)
            .HasForeignKey(s => s.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Asesor)
            .WithMany()
            .HasForeignKey(s => s.AsesorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // Índices no agrupados — cubre las consultas del punto 4.2 del PDF
        builder.HasIndex(s => s.Estado);
        builder.HasIndex(s => new { s.Estado, s.TipoApoyo, s.FechaSolicitud });
    }
}
