namespace EduApoyos.Domain.Common.Errors;

public static partial class DomainErrors
{
    public static class Solicitud
    {
        public static readonly Error EstadoInvalidoParaAsignar =
            new("Solicitud.EstadoInvalido", "Solo se puede asignar asesor a solicitudes en estado Pendiente.");

        public static readonly Error AsesorInvalido =
            new("Solicitud.AsesorInvalido", "El asesor asignado no es válido.");

        public static readonly Error EstadoInvalidoParaAprobar =
            new("Solicitud.EstadoInvalido", "Solo se pueden aprobar solicitudes en estado En Revisión.");

        public static readonly Error EstadoInvalidoParaRechazar =
            new("Solicitud.EstadoInvalido", "Solo se pueden rechazar solicitudes en estado En Revisión.");

        public static readonly Error AccesoDenegado =
            new("Solicitud.AccesoDenegado", "El estudiante no tiene acceso a esta solicitud.");
    }
}
