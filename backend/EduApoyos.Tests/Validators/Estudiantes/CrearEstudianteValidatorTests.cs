using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Application.Validators.Estudiantes;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Estudiantes;

public class CrearEstudianteValidatorTests
{
    private readonly CrearEstudianteValidator _validator = new();

    private static CrearEstudianteDto DtoValido() => new()
    {
        UsuarioId = Guid.NewGuid(),
        NumeroDocumento = "1012345678",
        TipoDocumento = TipoDocumento.CedulaCiudadania,
        ProgramaAcademico = "Ingenieria de Sistemas",
        Semestre = 4
    };

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(DtoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_UsuarioIdVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.UsuarioId = Guid.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.UsuarioId));
    }

    [Fact]
    public void Validate_NumeroDocumentoVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.NumeroDocumento = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.NumeroDocumento));
    }

    [Fact]
    public void Validate_NumeroDocumentoExcedeMaximo_EsInvalido()
    {
        var dto = DtoValido();
        dto.NumeroDocumento = new string('1', 21);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.NumeroDocumento));
    }

    [Fact]
    public void Validate_TipoDocumentoFueraDeEnum_EsInvalido()
    {
        var dto = DtoValido();
        dto.TipoDocumento = (TipoDocumento)999;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.TipoDocumento));
    }

    [Fact]
    public void Validate_ProgramaAcademicoVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.ProgramaAcademico = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.ProgramaAcademico));
    }

    [Fact]
    public void Validate_SemestreMenorDeUno_EsInvalido()
    {
        var dto = DtoValido();
        dto.Semestre = 0;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Semestre));
    }

    [Fact]
    public void Validate_SemestreMayorDeDoce_EsInvalido()
    {
        var dto = DtoValido();
        dto.Semestre = 13;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Semestre));
    }
}
