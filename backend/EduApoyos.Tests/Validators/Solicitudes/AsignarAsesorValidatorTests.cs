using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Application.Validators.Solicitudes;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class AsignarAsesorValidatorTests
{
    private readonly AsignarAsesorValidator _validator = new();

    private static AsignarAsesorDto DtoValido() => new()
    {
        SolicitudId = Guid.NewGuid(),
        AsesorId = Guid.NewGuid(),
        Observacion = "Asesor asignado según disponibilidad."
    };

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(DtoValido());

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
    public void Validate_AsesorIdVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.AsesorId = Guid.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.AsesorId));
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
