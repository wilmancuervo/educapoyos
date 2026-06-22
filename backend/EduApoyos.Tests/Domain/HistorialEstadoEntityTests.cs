using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Entities;

public class HistorialEstadoEntityTests
{
    [Fact]
    public void Constructor_MismoEstado_LanzaExcepcion()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new HistorialEstado(
                Guid.NewGuid(),
                Guid.NewGuid(),
                EstadoSolicitud.Pendiente,
                EstadoSolicitud.Pendiente,
                "Observacion"));
    }

    [Fact]
    public void Constructor_EstadosDiferentes_CreaHistorial()
    {
        var historial = new HistorialEstado(
            Guid.NewGuid(),
            Guid.NewGuid(),
            EstadoSolicitud.Pendiente,
            EstadoSolicitud.EnRevision,
            "Asignado para revisión");

        Assert.Equal(EstadoSolicitud.Pendiente, historial.EstadoAnterior);
        Assert.Equal(EstadoSolicitud.EnRevision, historial.EstadoNuevo);
        Assert.NotEqual(Guid.Empty, historial.Id);
    }
}
