using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Estudiantes;

public class CrearEstudianteCommandHandlerTests
{
    private readonly Mock<IEstudianteRepository> _estudianteRepo = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly CrearEstudianteCommandHandler _handler;

    public CrearEstudianteCommandHandlerTests()
    {
        _handler = new CrearEstudianteCommandHandler(_estudianteRepo.Object, _usuarioRepo.Object);
    }

    [Fact]
    public async Task Handle_UsuarioExisteYSinPerfil_CreaEstudianteYRetornaDto()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = DomainBuilders.BuildUsuario("Nuevo Estudiante", "nuevo@test.com", Rol.Estudiante);

        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync((Estudiante?)null);
        _estudianteRepo.Setup(r => r.AddAsync(It.IsAny<Estudiante>()))
            .Callback<Estudiante>(e => typeof(Estudiante).GetProperty("Usuario")!.SetValue(e, usuario));

        var result = await _handler.Handle(
            new CrearEstudianteCommand(usuarioId, "1012345678", TipoDocumento.CedulaCiudadania, "Ingenieria de Sistemas", 4), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(usuario.NombreCompleto, result.Value.NombreCompleto);
        _estudianteRepo.Verify(r => r.AddAsync(It.IsAny<Estudiante>()), Times.Once);
        _estudianteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioNoExiste_RetornaError()
    {
        _usuarioRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var result = await _handler.Handle(
            new CrearEstudianteCommand(Guid.NewGuid(), "1012345678", TipoDocumento.CedulaCiudadania, "Sistemas", 4), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Usuario.NoEncontrado.Code, result.Error.Code);
        _estudianteRepo.Verify(r => r.AddAsync(It.IsAny<Estudiante>()), Times.Never);
    }

    [Fact]
    public async Task Handle_PerfilYaExiste_RetornaError()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = DomainBuilders.BuildUsuario();
        var existente = DomainBuilders.BuildEstudiante(usuarioId, usuario);

        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(existente);

        var result = await _handler.Handle(
            new CrearEstudianteCommand(usuarioId, "1012345678", TipoDocumento.CedulaCiudadania, "Sistemas", 4), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Estudiante.YaExiste.Code, result.Error.Code);
        _estudianteRepo.Verify(r => r.AddAsync(It.IsAny<Estudiante>()), Times.Never);
    }
}
