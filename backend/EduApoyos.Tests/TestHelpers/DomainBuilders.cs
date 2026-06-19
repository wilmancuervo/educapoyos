using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.TestHelpers;

internal static class DomainBuilders
{
    internal static Usuario BuildUsuario(
        string nombre = "Test User",
        string email = "test@test.com",
        Rol rol = Rol.Estudiante) =>
        new(nombre, email, "hashed_password", rol);

    internal static Estudiante BuildEstudiante(Guid? usuarioId = null, Usuario? usuario = null)
    {
        var uid = usuarioId ?? Guid.NewGuid();
        var u = usuario ?? BuildUsuario();
        var estudiante = new Estudiante(uid, "1012345678", TipoDocumento.CedulaCiudadania, "Ingenieria de Sistemas", 4);
        typeof(Estudiante).GetProperty("Usuario")!.SetValue(estudiante, u);
        return estudiante;
    }

    internal static SolicitudApoyo BuildSolicitud(Estudiante? estudiante = null, TipoApoyo tipo = TipoApoyo.Beca)
    {
        var est = estudiante ?? BuildEstudiante();
        var solicitud = new SolicitudApoyo(est.Id, tipo, 1500000, "Solicitud de prueba");
        typeof(SolicitudApoyo).GetProperty("Estudiante")!.SetValue(solicitud, est);
        return solicitud;
    }

    internal static SolicitudApoyo BuildSolicitudEnRevision(Estudiante? estudiante = null)
    {
        var solicitud = BuildSolicitud(estudiante);
        solicitud.AsignarAsesor(Guid.NewGuid());
        return solicitud;
    }

    internal static HistorialEstado BuildHistorialEstado(Guid? solicitudId = null)
    {
        var asesor = BuildUsuario("Asesor Historial", "asesor@test.com", Rol.Asesor);
        var historial = new HistorialEstado(
            solicitudId ?? Guid.NewGuid(),
            Guid.NewGuid(),
            EstadoSolicitud.Pendiente,
            EstadoSolicitud.EnRevision,
            "Cambio de estado de prueba");
        typeof(HistorialEstado).GetProperty("Usuario")!.SetValue(historial, asesor);
        return historial;
    }

    internal static SolicitudApoyo BuildSolicitudConHistorial(Estudiante? estudiante = null)
    {
        var solicitud = BuildSolicitudEnRevision(estudiante);
        var historial = BuildHistorialEstado(solicitud.Id);
        solicitud.Historial.Add(historial);
        return solicitud;
    }

    internal static SolicitudApoyo BuildSolicitudConAsesor(Estudiante? estudiante = null)
    {
        var solicitud = BuildSolicitudEnRevision(estudiante);
        var asesor = BuildUsuario("Asesor Asignado", "asesorasig@test.com", Rol.Asesor);
        typeof(SolicitudApoyo).GetProperty("Asesor")!.SetValue(solicitud, asesor);
        return solicitud;
    }
}
