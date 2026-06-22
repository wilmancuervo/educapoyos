using EduApoyos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using EduApoyos.Application.Validators.Solicitudes;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class CrearSolicitudValidatorTests
{
    private readonly CrearSolicitudValidator _validator = new();

    private static CrearSolicitudCommand ComandoValido() =>
        new(Guid.NewGuid(), TipoApoyo.Beca, 1_500_000, "Solicitud de apoyo económico para matrícula.");

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(ComandoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_TipoApoyoFueraDeEnum_EsInvalido()
    {
        var cmd = ComandoValido() with { TipoApoyo = (TipoApoyo)999 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.TipoApoyo));
    }

    [Fact]
    public void Validate_MontoCero_EsInvalido()
    {
        var cmd = ComandoValido() with { MontoSolicitado = 0 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.MontoSolicitado));
    }

    [Fact]
    public void Validate_MontoNegativo_EsInvalido()
    {
        var cmd = ComandoValido() with { MontoSolicitado = -1000 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.MontoSolicitado));
    }

    [Fact]
    public void Validate_MontoSuperaLimite_EsInvalido()
    {
        var cmd = ComandoValido() with { MontoSolicitado = 100_000_001 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.MontoSolicitado));
    }

    [Fact]
    public void Validate_DescripcionVacia_EsInvalido()
    {
        var cmd = ComandoValido() with { Descripcion = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Descripcion));
    }

    [Fact]
    public void Validate_DescripcionExcedeMaximo_EsInvalido()
    {
        var cmd = ComandoValido() with { Descripcion = new string('x', 501) };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Descripcion));
    }
}
