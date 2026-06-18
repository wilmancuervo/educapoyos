using EduApoyos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduApoyos.Infrastructure.Persistence.Configurations;

public class HistorialEstadoConfiguration : IEntityTypeConfiguration<HistorialEstado>
{
    public void Configure(EntityTypeBuilder<HistorialEstado> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.EstadoAnterior)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(h => h.EstadoNuevo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(h => h.Observacion)
            .HasMaxLength(500);

        builder.Property(h => h.FechaCambio)
            .IsRequired();

        builder.HasOne(h => h.Solicitud)
            .WithMany(s => s.Historial)
            .HasForeignKey(h => h.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Usuario)
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
