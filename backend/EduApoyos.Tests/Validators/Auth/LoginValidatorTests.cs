using EduApoyos.Application.Features.Auth.Commands.Login;
using EduApoyos.Application.Validators.Auth;

namespace EduApoyos.Tests.Validators.Auth;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public void Validate_DatosValidos_EsValido()
    {
        var cmd = new LoginCommand("usuario@test.com", "secreto");

        var result = _validator.Validate(cmd);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_EmailVacio_EsInvalido()
    {
        var cmd = new LoginCommand(string.Empty, "secreto");

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Email));
    }

    [Fact]
    public void Validate_EmailFormatoIncorrecto_EsInvalido()
    {
        var cmd = new LoginCommand("no-es-un-email", "secreto");

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Email));
    }

    [Fact]
    public void Validate_PasswordVacio_EsInvalido()
    {
        var cmd = new LoginCommand("usuario@test.com", string.Empty);

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Password));
    }
}
