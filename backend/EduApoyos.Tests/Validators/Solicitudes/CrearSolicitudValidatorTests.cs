using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Application.Validators.Solicitudes;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Solicitudes;

public class CrearSolicitudValidatorTests
{
    private readonly CrearSolicitudValidator _validator = new();

    private static CrearSolicitudDto DtoValido() => new()
    {
        EstudianteId = Guid.NewGuid(),
        TipoApoyo = TipoApoyo.Beca,
        MontoSolicitado = 1_500_000,
        Descripcion = "Solicitud de apoyo económico para matrícula."
    };

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(DtoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_TipoApoyoFueraDeEnum_EsInvalido()
    {
        var dto = DtoValido();
        dto.TipoApoyo = (TipoApoyo)999;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.TipoApoyo));
    }

    [Fact]
    public void Validate_MontoCero_EsInvalido()
    {
        var dto = DtoValido();
        dto.MontoSolicitado = 0;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.MontoSolicitado));
    }

    [Fact]
    public void Validate_MontoNegativo_EsInvalido()
    {
        var dto = DtoValido();
        dto.MontoSolicitado = -1000;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.MontoSolicitado));
    }

    [Fact]
    public void Validate_MontoSuperaLimite_EsInvalido()
    {
        var dto = DtoValido();
        dto.MontoSolicitado = 100_000_001;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.MontoSolicitado));
    }

    [Fact]
    public void Validate_DescripcionVacia_EsInvalido()
    {
        var dto = DtoValido();
        dto.Descripcion = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Descripcion));
    }

    [Fact]
    public void Validate_DescripcionExcedeMaximo_EsInvalido()
    {
        var dto = DtoValido();
        dto.Descripcion = new string('x', 501);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Descripcion));
    }
}
