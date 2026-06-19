using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Auth.Commands.Register;
using EduApoyos.Application.Interfaces;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;

namespace EduApoyos.Tests.Features.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _handler = new RegisterCommandHandler(_usuarioRepo.Object, _hasher.Object, _jwtService.Object);
    }

    [Fact]
    public async Task Handle_EmailNuevo_RegistraUsuarioYRetornaToken()
    {
        _usuarioRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
        _hasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed_password");
        _jwtService.Setup(j => j.GenerarToken(It.IsAny<Usuario>())).Returns("jwt_token");

        var result = await _handler.Handle(
            new RegisterCommand("Nuevo Usuario", "nuevo@test.com", "Password@123", Rol.Estudiante), default);

        Assert.True(result.IsSuccess);
        Assert.Equal("jwt_token", result.Value.Token);
        _usuarioRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Once);
        _usuarioRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EmailDuplicado_RetornaEmailYaRegistrado()
    {
        var existente = new Usuario("Existente", "existe@test.com", "hash", Rol.Estudiante);
        _usuarioRepo.Setup(r => r.GetByEmailAsync("existe@test.com")).ReturnsAsync(existente);

        var result = await _handler.Handle(
            new RegisterCommand("Nuevo", "existe@test.com", "Password@123", Rol.Estudiante), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Auth.EmailYaRegistrado.Code, result.Error.Code);
        _usuarioRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Never);
    }
}
