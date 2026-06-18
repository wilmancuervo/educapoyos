using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class HistorialEstado
{
    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public EstadoSolicitud EstadoAnterior { get; private set; }
    public EstadoSolicitud EstadoNuevo { get; private set; }
    public string Observacion { get; private set; } = string.Empty;
    public DateTime FechaCambio { get; private set; }

    public SolicitudApoyo Solicitud { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;

    protected HistorialEstado() { }

    public HistorialEstado(Guid solicitudId, Guid usuarioId, EstadoSolicitud anterior, EstadoSolicitud nuevo, string observacion)
    {
        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        UsuarioId = usuarioId;
        EstadoAnterior = anterior;
        EstadoNuevo = nuevo;
        Observacion = observacion;
        FechaCambio = DateTime.UtcNow;
    }
}
