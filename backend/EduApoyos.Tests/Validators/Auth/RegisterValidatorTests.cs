using EduApoyos.Application.Features.Auth.Commands.Register;
using EduApoyos.Application.Validators.Auth;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Auth;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator = new();

    private static RegisterCommand ComandoValido() =>
        new("Juan Pérez", "juan@test.com", "Segura123", Rol.Estudiante);

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(ComandoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_NombreCompletoVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { NombreCompleto = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.NombreCompleto));
    }

    [Fact]
    public void Validate_NombreCompletoExcedeMaximo_EsInvalido()
    {
        var cmd = ComandoValido() with { NombreCompleto = new string('A', 151) };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.NombreCompleto));
    }

    [Fact]
    public void Validate_EmailVacio_EsInvalido()
    {
        var cmd = ComandoValido() with { Email = string.Empty };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Email));
    }

    [Fact]
    public void Validate_EmailFormatoIncorrecto_EsInvalido()
    {
        var cmd = ComandoValido() with { Email = "no-es-email" };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Email));
    }

    [Fact]
    public void Validate_PasswordSinMayuscula_EsInvalido()
    {
        var cmd = ComandoValido() with { Password = "sinmayuscula1" };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Password));
    }

    [Fact]
    public void Validate_PasswordSinNumero_EsInvalido()
    {
        var cmd = ComandoValido() with { Password = "SinNumeroAqui" };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Password));
    }

    [Fact]
    public void Validate_PasswordMenorDe8Caracteres_EsInvalido()
    {
        var cmd = ComandoValido() with { Password = "Cor1" };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Password));
    }

    [Fact]
    public void Validate_RolFueraDeEnum_EsInvalido()
    {
        var cmd = ComandoValido() with { Rol = (Rol)999 };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Rol));
    }
}
