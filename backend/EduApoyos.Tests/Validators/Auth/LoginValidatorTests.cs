using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.Validators.Auth;

namespace EduApoyos.Tests.Validators.Auth;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var dto = new LoginDto { Email = "usuario@test.com", Password = "secreto" };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_EmailVacio_EsInvalido()
    {
        var dto = new LoginDto { Email = string.Empty, Password = "secreto" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Email));
    }

    [Fact]
    public void Validate_EmailFormatoIncorrecto_EsInvalido()
    {
        var dto = new LoginDto { Email = "no-es-un-email", Password = "secreto" };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Email));
    }

    [Fact]
    public void Validate_PasswordVacio_EsInvalido()
    {
        var dto = new LoginDto { Email = "usuario@test.com", Password = string.Empty };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Password));
    }
}
