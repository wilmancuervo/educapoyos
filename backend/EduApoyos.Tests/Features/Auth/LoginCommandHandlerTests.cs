using EduApoyos.Application.Features.Auth.Commands.Login;
using EduApoyos.Application.Interfaces;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;

namespace EduApoyos.Tests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(_usuarioRepo.Object, _hasher.Object, _jwtService.Object);
    }

    [Fact]
    public async Task Handle_CredencialesValidas_RetornaToken()
    {
        var usuario = new Usuario("Test User", "test@test.com", "hash", Rol.Estudiante);
        _usuarioRepo.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("password123", "hash")).Returns(true);
        _jwtService.Setup(j => j.GenerarToken(usuario)).Returns("jwt_token");

        var result = await _handler.Handle(new LoginCommand("test@test.com", "password123"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal("jwt_token", result.Value.Token);
    }

    [Fact]
    public async Task Handle_UsuarioNoExiste_RetornaFailure()
    {
        _usuarioRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario?)null);

        var result = await _handler.Handle(new LoginCommand("noexiste@test.com", "password"), default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_PasswordIncorrecta_RetornaFailure()
    {
        var usuario = new Usuario("Test User", "test@test.com", "hash", Rol.Estudiante);
        _usuarioRepo.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("wrongpassword", "hash")).Returns(false);

        var result = await _handler.Handle(new LoginCommand("test@test.com", "wrongpassword"), default);

        Assert.True(result.IsFailure);
    }
}
