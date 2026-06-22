using EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Features.Estudiantes;

public class ListarEstudiantesQueryHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly ListarEstudiantesQueryHandler _handler;

    public ListarEstudiantesQueryHandlerTests()
    {
        _handler = new ListarEstudiantesQueryHandler(_usuarioRepo.Object);
    }

    [Fact]
    public async Task Handle_ConEstudiantes_RetornaListaPaginada()
    {
        var usuarios = new List<Usuario> { DomainBuilders.BuildUsuario(), DomainBuilders.BuildUsuario() };
        _usuarioRepo.Setup(r => r.GetEstudiantesPagedAsync(1, 10)).ReturnsAsync((usuarios, 2));

        var result = await _handler.Handle(new ListarEstudiantesQuery(1, 10), default);

        Assert.Equal(2, result.Total);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task Handle_SinEstudiantes_RetornaResultadoVacio()
    {
        _usuarioRepo.Setup(r => r.GetEstudiantesPagedAsync(1, 10)).ReturnsAsync((new List<Usuario>(), 0));

        var result = await _handler.Handle(new ListarEstudiantesQuery(1, 10), default);

        Assert.Equal(0, result.Total);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Handle_SegundaPagina_RetornaMetadatosDePaginaCorrectos()
    {
        var usuarios = new List<Usuario> { DomainBuilders.BuildUsuario() };
        _usuarioRepo.Setup(r => r.GetEstudiantesPagedAsync(2, 5)).ReturnsAsync((usuarios, 6));

        var result = await _handler.Handle(new ListarEstudiantesQuery(2, 5), default);

        Assert.Equal(6, result.Total);
        Assert.Equal(2, result.Page);
        Assert.Equal(5, result.PageSize);
    }
}
