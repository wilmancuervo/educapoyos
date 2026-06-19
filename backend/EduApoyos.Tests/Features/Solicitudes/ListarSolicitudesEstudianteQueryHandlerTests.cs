using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class ListarSolicitudesEstudianteQueryHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly Mock<IEstudianteRepository> _estudianteRepo = new();
    private readonly ListarSolicitudesEstudianteQueryHandler _handler;

    public ListarSolicitudesEstudianteQueryHandlerTests()
    {
        _handler = new ListarSolicitudesEstudianteQueryHandler(_solicitudRepo.Object, _estudianteRepo.Object);
    }

    [Fact]
    public async Task Handle_EstudianteConSolicitudes_RetornaLista()
    {
        var usuarioId = Guid.NewGuid();
        var estudiante = DomainBuilders.BuildEstudiante(usuarioId);
        var solicitudes = new List<SolicitudApoyo>
        {
            DomainBuilders.BuildSolicitud(estudiante),
            DomainBuilders.BuildSolicitud(estudiante)
        };

        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(estudiante);
        _solicitudRepo.Setup(r => r.GetByEstudianteIdAsync(estudiante.Id)).ReturnsAsync(solicitudes);

        var result = await _handler.Handle(new ListarSolicitudesEstudianteQuery(usuarioId), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Handle_EstudianteSinSolicitudes_RetornaListaVacia()
    {
        var usuarioId = Guid.NewGuid();
        var estudiante = DomainBuilders.BuildEstudiante(usuarioId);

        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(usuarioId)).ReturnsAsync(estudiante);
        _solicitudRepo.Setup(r => r.GetByEstudianteIdAsync(estudiante.Id)).ReturnsAsync(new List<SolicitudApoyo>());

        var result = await _handler.Handle(new ListarSolicitudesEstudianteQuery(usuarioId), default);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_EstudianteNoExiste_RetornaError()
    {
        _estudianteRepo.Setup(r => r.GetByUsuarioIdAsync(It.IsAny<Guid>())).ReturnsAsync((Estudiante?)null);

        var result = await _handler.Handle(new ListarSolicitudesEstudianteQuery(Guid.NewGuid()), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.Estudiante.NoEncontrado.Code, result.Error.Code);
        _solicitudRepo.Verify(r => r.GetByEstudianteIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}
