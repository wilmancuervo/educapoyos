using EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;
using EduApoyos.Application.Validators.Solicitudes;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class AsignarAsesorValidatorTests
{
    private readonly AsignarAsesorValidator _validator = new();

    private static AsignarAsesorCommand ComandoValido() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Asesor asignado según disponibilidad.");

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(ComandoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_SolicitudIdVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { SolicitudId = Guid.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.SolicitudId));
    }

    [Fact]
    public void Validate_AsesorIdVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { AsesorId = Guid.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.AsesorId));
    }

    [Fact]
    public void Validate_ObservacionVacia_EsInvalido()
    {
        var cmd = ComandoValido() with { Observacion = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Observacion));
    }

    [Fact]
    public void Validate_ObservacionExcedeMaximo_EsInvalido()
    {
        var cmd = ComandoValido() with { Observacion = new string('o', 501) };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Observacion));
    }
}
