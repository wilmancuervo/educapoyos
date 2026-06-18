namespace EduApoyos.Domain.Common.Errors;

public static partial class DomainErrors
{
    public static class HistorialEstado
    {
        public static readonly Error SinCambioDeEstado =
            new("HistorialEstado.SinCambio", "El estado nuevo debe ser diferente al estado anterior.");
    }
}
