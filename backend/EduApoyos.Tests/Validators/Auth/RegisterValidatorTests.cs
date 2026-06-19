using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Validators.Auth;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Validators.Auth;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator = new();

    private static RegisterDto DtoValido() => new()
    {
        NombreCompleto = "Juan Pérez",
        Email = "juan@test.com",
        Password = "Segura123",
        Rol = Rol.Estudiante
    };

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var result = _validator.Validate(DtoValido());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_NombreCompletoVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.NombreCompleto = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.NombreCompleto));
    }

    [Fact]
    public void Validate_NombreCompletoExcedeMaximo_EsInvalido()
    {
        var dto = DtoValido();
        dto.NombreCompleto = new string('A', 151);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.NombreCompleto));
    }

    [Fact]
    public void Validate_EmailVacio_EsInvalido()
    {
        var dto = DtoValido();
        dto.Email = string.Empty;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Email));
    }

    [Fact]
    public void Validate_EmailFormatoIncorrecto_EsInvalido()
    {
        var dto = DtoValido();
        dto.Email = "no-es-email";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Email));
    }

    [Fact]
    public void Validate_PasswordSinMayuscula_EsInvalido()
    {
        var dto = DtoValido();
        dto.Password = "sinmayuscula1";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Password));
    }

    [Fact]
    public void Validate_PasswordSinNumero_EsInvalido()
    {
        var dto = DtoValido();
        dto.Password = "SinNumeroAqui";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Password));
    }

    [Fact]
    public void Validate_PasswordMenorDe8Caracteres_EsInvalido()
    {
        var dto = DtoValido();
        dto.Password = "Cor1";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Password));
    }

    [Fact]
    public void Validate_RolFueraDeEnum_EsInvalido()
    {
        var dto = DtoValido();
        dto.Rol = (Rol)999;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Rol));
    }
}
