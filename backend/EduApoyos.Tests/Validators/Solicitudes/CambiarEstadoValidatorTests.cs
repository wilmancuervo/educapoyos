using EduApoyos.Application.Features.Solicitudes.Commands.CambiarEstado;
using EduApoyos.Application.Validators.Solicitudes;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class CambiarEstadoValidatorTests
{
    private readonly CambiarEstadoValidator _validator = new();

    private static CambiarEstadoCommand ComandoValido() =>
        new(Guid.NewGuid(), "aprobar", Guid.NewGuid(), "Cumple todos los requisitos establecidos.");

    [Fact]
    public void Validate_AprobarValido_EsValido()
    {
        var result = _validator.Validate(ComandoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_RechazarValido_EsValido()
    {
        var cmd = ComandoValido() with { Accion = "rechazar" };

        var result = _validator.Validate(cmd);

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
    public void Validate_AccionVacia_EsInvalido()
    {
        var cmd = ComandoValido() with { Accion = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Accion));
    }

    [Fact]
    public void Validate_AccionNoReconocida_EsInvalido()
    {
        var cmd = ComandoValido() with { Accion = "cancelar" };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Accion));
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
