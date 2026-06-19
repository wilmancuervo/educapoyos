using EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudes;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Solicitudes;

public class ListarSolicitudesQueryHandlerTests
{
    private readonly Mock<ISolicitudApoyoRepository> _solicitudRepo = new();
    private readonly ListarSolicitudesQueryHandler _handler;

    public ListarSolicitudesQueryHandlerTests()
    {
        _handler = new ListarSolicitudesQueryHandler(_solicitudRepo.Object);
    }

    [Fact]
    public async Task Handle_SinFiltros_RetornaListaPaginada()
    {
        var solicitudes = new List<SolicitudApoyo>
        {
            DomainBuilders.BuildSolicitud(),
            DomainBuilders.BuildSolicitud()
        };
        _solicitudRepo.Setup(r => r.GetPagedAsync(1, 10,
            It.IsAny<EstadoSolicitud?>(), It.IsAny<TipoApoyo?>(),
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((solicitudes, 2));

        var result = await _handler.Handle(new ListarSolicitudesQuery(1, 10), default);

        Assert.Equal(2, result.Total);
        Assert.Equal(2, result.Items.Count());
    }

    [Fact]
    public async Task Handle_SinSolicitudes_RetornaListaVacia()
    {
        _solicitudRepo.Setup(r => r.GetPagedAsync(1, 10,
            It.IsAny<EstadoSolicitud?>(), It.IsAny<TipoApoyo?>(),
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((new List<SolicitudApoyo>(), 0));

        var result = await _handler.Handle(new ListarSolicitudesQuery(1, 10), default);

        Assert.Equal(0, result.Total);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Handle_ConFiltroEstado_RetornaListaFiltrada()
    {
        var solicitud = DomainBuilders.BuildSolicitud();
        _solicitudRepo.Setup(r => r.GetPagedAsync(1, 10,
            EstadoSolicitud.Pendiente, It.IsAny<TipoApoyo?>(),
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((new List<SolicitudApoyo> { solicitud }, 1));

        var result = await _handler.Handle(new ListarSolicitudesQuery(1, 10, EstadoSolicitud.Pendiente), default);

        Assert.Equal(1, result.Total);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task Handle_ConFiltroTipo_RetornaListaFiltrada()
    {
        var solicitud = DomainBuilders.BuildSolicitud(tipo: TipoApoyo.Credito);
        _solicitudRepo.Setup(r => r.GetPagedAsync(1, 10,
            It.IsAny<EstadoSolicitud?>(), TipoApoyo.Credito,
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((new List<SolicitudApoyo> { solicitud }, 1));

        var result = await _handler.Handle(new ListarSolicitudesQuery(1, 10, Tipo: TipoApoyo.Credito), default);

        Assert.Equal(1, result.Total);
        Assert.Equal(TipoApoyo.Credito.ToString(), result.Items.First().TipoApoyo);
    }
}
