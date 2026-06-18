using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Entities;

public class SolicitudApoyo
{
    public Guid Id { get; private set; }
    public Guid EstudianteId { get; private set; }
    public Guid? AsesorId { get; private set; }
    public TipoApoyo TipoApoyo { get; private set; }
    public decimal MontoSolicitado { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public EstadoSolicitud Estado { get; private set; } = EstadoSolicitud.Pendiente;
    public DateTime FechaSolicitud { get; private set; }
    public DateTime FechaActualizacion { get; private set; }

    public Estudiante Estudiante { get; private set; } = null!;
    public Usuario? Asesor { get; private set; }
    public ICollection<HistorialEstado> Historial { get; private set; } = [];

    public void AsignarAsesor(Guid asesorId)
    {
        AsesorId = asesorId;
        Estado = EstadoSolicitud.EnRevision;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void Aprobar()
    {
        Estado = EstadoSolicitud.Aprobada;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void Rechazar()
    {
        Estado = EstadoSolicitud.Rechazada;
        FechaActualizacion = DateTime.UtcNow;
    }

    protected SolicitudApoyo() { }

    public SolicitudApoyo(Guid estudianteId, TipoApoyo tipoApoyo, decimal monto, string descripcion)
    {
        Id = Guid.NewGuid();
        EstudianteId = estudianteId;
        TipoApoyo = tipoApoyo;
        MontoSolicitado = monto;
        Descripcion = descripcion;
        Estado = EstadoSolicitud.Pendiente;
        FechaSolicitud = DateTime.UtcNow;
        FechaActualizacion = DateTime.UtcNow;
    }
}
