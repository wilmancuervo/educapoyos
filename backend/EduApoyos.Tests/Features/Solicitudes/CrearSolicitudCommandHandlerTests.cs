using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class CrearSolicitudCommandHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly Mock<IEstudianteRepository> _estudianteRepo = new();
    private readonly CrearSolicitudCommandHandler _handler;

    public CrearSolicitudCommandHandlerTests()
    {
        _handler = new CrearSolicitudCommandHandler(_solicitudRepo.Object, _estudianteRepo.Object);
    }

    [Fact]
    public async Task Handle_EstudianteExiste_CreaYRetornaSolicitud()
    {
        var usuarioId = Guid.NewGuid();
        var estudiante = DomainBuilders.BuildEstudiante(usuarioId);

        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(estudiante);

        // cuando AddAsync es llamado, setea la navegación Estudiante en la solicitud nueva
        _solicitudRepo.Setup(r => r.AddAsync(It.IsAny<SolicitudApoyo>()))
            .Callback<SolicitudApoyo>(s =>
                typeof(SolicitudApoyo).GetProperty("Estudiante")!.SetValue(s, estudiante));

        var result = await _handler.Handle(
            new CrearSolicitudCommand(usuarioId, TipoApoyo.Beca, 1500000, "Solicitud de beca por mérito"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(TipoApoyo.Beca.ToString(), result.Value.TipoApoyo);
        _solicitudRepo.Verify(r => r.AddAsync(It.IsAny<SolicitudApoyo>()), Times.Once);
        _solicitudRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EstudianteNoExiste_RetornaError()
    {
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(It.IsAny<Guid>())).ReturnsAsync((Estudiante?)null);

        var result = await _handler.Handle(
            new CrearSolicitudCommand(Guid.NewGuid(), TipoApoyo.Credito, 2000000, "Solicitud de credito"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Estudiante.NoEncontrado.Code, result.Error.Code);
        _solicitudRepo.Verify(r => r.AddAsync(It.IsAny<SolicitudApoyo>()), Times.Never);
    }
}
