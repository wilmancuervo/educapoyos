using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class AsignarAsesorCommandHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly AsignarAsesorCommandHandler _handler;

    public AsignarAsesorCommandHandlerTests()
    {
        _handler = new AsignarAsesorCommandHandler(_solicitudRepo.Object, _usuarioRepo.Object);
    }

    [Fact]
    public async Task Handle_AsesorValido_AsignaYRetornaSuccess()
    {
        var asesorId = Guid.NewGuid();
        var asesor = new Usuario("Asesor Test", "asesor@test.com", "hash", Rol.Asesor);
        var solicitud = new SolicitudApoyo(Guid.NewGuid(), TipoApoyo.Beca, 1500000, "Test");

        _usuarioRepo.Setup(r => r.GetByIdAsync(asesorId)).ReturnsAsync(asesor);
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new AsignarAsesorCommand(solicitud.Id, asesorId, Guid.NewGuid(), "Asignado para revisión"), default);

        Assert.True(result.IsSuccess);
        _solicitudRepo.Verify(r => r.AddHistorialAsync(It.IsAny<HistorialEstado>()), Times.Once);
        _solicitudRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_AsesorNoExiste_RetornaError()
    {
        _usuarioRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var result = await _handler.Handle(
            new AsignarAsesorCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Usuario.NoEncontrado.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_UsuarioNoEsAsesor_RetornaError()
    {
        var estudianteId = Guid.NewGuid();
        var usuarioEstudiante = new Usuario("Estudiante Test", "estudiante@test.com", "hash", Rol.Estudiante);
        _usuarioRepo.Setup(r => r.GetByIdAsync(estudianteId)).ReturnsAsync(usuarioEstudiante);

        var result = await _handler.Handle(
            new AsignarAsesorCommand(Guid.NewGuid(), estudianteId, Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Usuario.NoEsAsesor.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_SolicitudNoExiste_RetornaError()
    {
        var asesorId = Guid.NewGuid();
        var asesor = new Usuario("Asesor Test", "asesor@test.com", "hash", Rol.Asesor);
        _usuarioRepo.Setup(r => r.GetByIdAsync(asesorId)).ReturnsAsync(asesor);
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(It.IsAny<Guid>())).ReturnsAsync((SolicitudApoyo?)null);

        var result = await _handler.Handle(
            new AsignarAsesorCommand(Guid.NewGuid(), asesorId, Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Solicitud.NoEncontrada.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_SolicitudYaEnRevision_RetornaError()
    {
        var asesorId = Guid.NewGuid();
        var asesor = new Usuario("Asesor Test", "asesor@test.com", "hash", Rol.Asesor);
        var solicitudEnRevision = DomainBuilders.BuildSolicitudEnRevision(); // ya tiene asesor, estado EnRevision

        _usuarioRepo.Setup(r => r.GetByIdAsync(asesorId)).ReturnsAsync(asesor);
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitudEnRevision.Id)).ReturnsAsync(solicitudEnRevision);

        var result = await _handler.Handle(
            new AsignarAsesorCommand(solicitudEnRevision.Id, asesorId, Guid.NewGuid(), "Segundo asesor"), default);

        Assert.True(result.IsFailure);
        _solicitudRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
