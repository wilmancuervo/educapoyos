using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Application.Validators.Solicitudes;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class CambiarEstadoValidatorTests
{
    private readonly CambiarEstadoValidator _validator = new();

    private static CambiarEstadoDto DtoValido() => new()
    {
        SolicitudId = Guid.NewGuid(),
        Accion = "aprobar",
        Observacion = "Cumple todos los requisitos establecidos."
    };

    [Fact]
    public void Validate_AprobarValido_EsValido()
    {
        var result = _validator.Validate(DtoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_RechazarValido_EsValido()
    {
        var dto = DtoValido();
        dto.Accion = "rechazar";

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_SolicitudIdVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.SolicitudId = Guid.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.SolicitudId));
    }

    [Fact]
    public void Validate_AccionVacia_EsInvalido()
    {
        var dto = DtoValido();
        dto.Accion = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Accion));
    }

    [Fact]
    public void Validate_AccionNoReconocida_EsInvalido()
    {
        var dto = DtoValido();
        dto.Accion = "cancelar";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Accion));
    }

    [Fact]
    public void Validate_ObservacionVacia_EsInvalido()
    {
        var dto = DtoValido();
        dto.Observacion = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Observacion));
    }

    [Fact]
    public void Validate_ObservacionExcedeMaximo_EsInvalido()
    {
        var dto = DtoValido();
        dto.Observacion = new string('o', 501);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Observacion));
    }
}
