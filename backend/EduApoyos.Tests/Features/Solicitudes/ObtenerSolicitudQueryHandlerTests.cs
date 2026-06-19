using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Solicitudes.Queries.ObtenerSolicitud;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class ObtenerSolicitudQueryHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly Mock<IEstudianteRepository> _estudianteRepo = new();
    private readonly ObtenerSolicitudQueryHandler _handler;

    public ObtenerSolicitudQueryHandlerTests()
    {
        _handler = new ObtenerSolicitudQueryHandler(_solicitudRepo.Object, _estudianteRepo.Object);
    }

    [Fact]
    public async Task Handle_AsesorConsultaCualquierSolicitud_RetornaSuccess()
    {
        var solicitud = DomainBuilders.BuildSolicitud();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, Guid.NewGuid(), Rol.Asesor), default);

        Assert.True(result.IsSuccess);
        _estudianteRepo.Verify(r => r.GetByUsuarioIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EstudianteConsultaSuPropiaSolicitud_RetornaSuccess()
    {
        var usuarioId = Guid.NewGuid();
        var estudiante = DomainBuilders.BuildEstudiante(usuarioId);
        var solicitud = DomainBuilders.BuildSolicitud(estudiante);

        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(estudiante);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, usuarioId, Rol.Estudiante), default);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EstudianteConsultaSolicitudAjena_RetornaAccesoDenegado()
    {
        var usuarioId = Guid.NewGuid();
        var estudianteAjeno = DomainBuilders.BuildEstudiante(usuarioId);
        var solicitud = DomainBuilders.BuildSolicitud(); // pertenece a otro estudiante

        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(estudianteAjeno);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, usuarioId, Rol.Estudiante), default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_SolicitudNoExiste_RetornaError()
    {
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(It.IsAny<Guid>()))
            .ReturnsAsync((SolicitudApoyo?)null);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(Guid.NewGuid(), Guid.NewGuid(), Rol.Asesor), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Solicitud.NoEncontrada.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_EstudianteNoEncontradoParaRolEstudiante_RetornaAccesoDenegado()
    {
        var solicitud = DomainBuilders.BuildSolicitud();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Estudiante?)null);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, Guid.NewGuid(), Rol.Estudiante), default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_SolicitudConAsesorAsignado_IncluyeNombreAsesorEnDto()
    {
        var solicitud = DomainBuilders.BuildSolicitudConAsesor();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, Guid.NewGuid(), Rol.Asesor), default);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value.NombreAsesor);
    }

    [Fact]
    public async Task Handle_SolicitudConHistorial_IncluyeHistorialEnDto()
    {
        var solicitud = DomainBuilders.BuildSolicitudConHistorial();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new ObtenerSolicitudQuery(solicitud.Id, Guid.NewGuid(), Rol.Asesor), default);

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Historial);
    }
}
