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
        if (Estado != EstadoSolicitud.Pendiente)
            throw new InvalidOperationException("Solo se puede asignar asesor a solicitudes en estado Pendiente.");

        if (asesorId == Guid.Empty)
            throw new ArgumentException("El asesor no es válido.");

        AsesorId = asesorId;
        Estado = EstadoSolicitud.EnRevision;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void Aprobar()
    {
        if (Estado != EstadoSolicitud.EnRevision)
            throw new InvalidOperationException("Solo se pueden aprobar solicitudes en estado En Revisión.");

        Estado = EstadoSolicitud.Aprobada;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void Rechazar()
    {
        if (Estado != EstadoSolicitud.EnRevision)
            throw new InvalidOperationException("Solo se pueden rechazar solicitudes en estado En Revisión.");

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
