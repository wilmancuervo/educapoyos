using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Solicitudes.Commands.CambiarEstado;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class CambiarEstadoCommandHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly CambiarEstadoCommandHandler _handler;

    public CambiarEstadoCommandHandlerTests()
    {
        _handler = new CambiarEstadoCommandHandler(_solicitudRepo.Object);
    }

    [Fact]
    public async Task Handle_AprobarSolicitudEnRevision_RetornaSuccess()
    {
        var solicitud = DomainBuilders.BuildSolicitudEnRevision();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(solicitud.Id, "aprobar", Guid.NewGuid(), "Aprobada por mérito"), default);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_RechazarSolicitudEnRevision_RetornaSuccess()
    {
        var solicitud = DomainBuilders.BuildSolicitudEnRevision();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(solicitud.Id, "rechazar", Guid.NewGuid(), "No cumple requisitos"), default);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_SolicitudNoExiste_RetornaError()
    {
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.SolicitudApoyo?)null);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(Guid.NewGuid(), "aprobar", Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Solicitud.NoEncontrada.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_AccionInvalida_RetornaError()
    {
        var solicitud = DomainBuilders.BuildSolicitudEnRevision();
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(solicitud.Id, "accion_desconocida", Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Solicitud.AccionInvalida.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_AprobarSolicitudPendiente_RetornaError()
    {
        var solicitud = DomainBuilders.BuildSolicitud(); // estado Pendiente, no EnRevision
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(solicitud.Id, "aprobar", Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        _solicitudRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_RechazarSolicitudPendiente_RetornaError()
    {
        var solicitud = DomainBuilders.BuildSolicitud(); // estado Pendiente, no EnRevision
        _solicitudRepo.Setup(r => r.GetByIdWithHistorialAsync(solicitud.Id)).ReturnsAsync(solicitud);

        var result = await _handler.Handle(
            new CambiarEstadoCommand(solicitud.Id, "rechazar", Guid.NewGuid(), "Observacion"), default);

        Assert.True(result.IsFailure);
        _solicitudRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
