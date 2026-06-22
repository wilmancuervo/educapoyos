using EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;
using EduApoyos.Application.Validators.Estudiantes;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Estudiantes;

public class CrearEstudianteValidatorTests
{
    private readonly CrearEstudianteValidator _validator = new();

    private static CrearEstudianteCommand ComandoValido() =>
        new(Guid.NewGuid(), "1012345678", TipoDocumento.CedulaCiudadania, "Ingenieria de Sistemas", 4);

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(ComandoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_UsuarioIdVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { UsuarioId = Guid.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.UsuarioId));
    }

    [Fact]
    public void Validate_NumeroDocumentoVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { NumeroDocumento = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.NumeroDocumento));
    }

    [Fact]
    public void Validate_NumeroDocumentoExcedeMaximo_EsInvalido()
    {
        var cmd = ComandoValido() with { NumeroDocumento = new string('1', 21) };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.NumeroDocumento));
    }

    [Fact]
    public void Validate_TipoDocumentoFueraDeEnum_EsInvalido()
    {
        var cmd = ComandoValido() with { TipoDocumento = (TipoDocumento)999 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.TipoDocumento));
    }

    [Fact]
    public void Validate_ProgramaAcademicoVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { ProgramaAcademico = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.ProgramaAcademico));
    }

    [Fact]
    public void Validate_SemestreMenorDeUno_EsInvalido()
    {
        var cmd = ComandoValido() with { Semestre = 0 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Semestre));
    }

    [Fact]
    public void Validate_SemestreMayorDeDoce_EsInvalido()
    {
        var cmd = ComandoValido() with { Semestre = 13 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Semestre));
    }
}
