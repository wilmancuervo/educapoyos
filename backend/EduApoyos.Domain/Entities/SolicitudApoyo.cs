using EduApoyos.Domain.Common;
using EduApoyos.Domain.Common.Errors;
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

    public bool PerteneceA(Guid estudianteId) => EstudianteId == estudianteId;

    public Result AsignarAsesor(Guid asesorId)
    {
        if (Estado != EstadoSolicitud.Pendiente)
            return Result.Failure(DomainErrors.Solicitud.EstadoInvalidoParaAsignar);

        if (asesorId == Guid.Empty)
            return Result.Failure(DomainErrors.Solicitud.AsesorInvalido);

        AsesorId = asesorId;
        Estado = EstadoSolicitud.EnRevision;
        FechaActualizacion = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Aprobar()
    {
        if (Estado != EstadoSolicitud.EnRevision)
            return Result.Failure(DomainErrors.Solicitud.EstadoInvalidoParaAprobar);

        Estado = EstadoSolicitud.Aprobada;
        FechaActualizacion = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Rechazar()
    {
        if (Estado != EstadoSolicitud.EnRevision)
            return Result.Failure(DomainErrors.Solicitud.EstadoInvalidoParaRechazar);

        Estado = EstadoSolicitud.Rechazada;
        FechaActualizacion = DateTime.UtcNow;
        return Result.Success();
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
