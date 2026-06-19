using EduApoyos.Application.DTOs.Auth;
using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Common;

public class InputDtoTests
{
    [Fact]
    public void LoginDto_PropiedadesAsignables()
    {
        var dto = new LoginDto { Email = "test@test.com", Password = "pass123" };

        Assert.Equal("test@test.com", dto.Email);
        Assert.Equal("pass123", dto.Password);
    }

    [Fact]
    public void RegisterDto_PropiedadesAsignables()
    {
        var dto = new RegisterDto { NombreCompleto = "Test User", Email = "t@t.com", Password = "p123", Rol = Rol.Asesor };

        Assert.Equal("Test User", dto.NombreCompleto);
        Assert.Equal(Rol.Asesor, dto.Rol);
    }

    [Fact]
    public void CrearEstudianteDto_PropiedadesAsignables()
    {
        var id = Guid.NewGuid();
        var dto = new CrearEstudianteDto
        {
            UsuarioId = id,
            NumeroDocumento = "1012345678",
            TipoDocumento = TipoDocumento.CedulaCiudadania,
            ProgramaAcademico = "Ingenieria de Sistemas",
            Semestre = 4
        };

        Assert.Equal(id, dto.UsuarioId);
        Assert.Equal(TipoDocumento.CedulaCiudadania, dto.TipoDocumento);
        Assert.Equal(4, dto.Semestre);
    }

    [Fact]
    public void CrearSolicitudDto_PropiedadesAsignables()
    {
        var dto = new CrearSolicitudDto
        {
            EstudianteId = Guid.NewGuid(),
            TipoApoyo = TipoApoyo.Beca,
            MontoSolicitado = 1500000,
            Descripcion = "Solicitud de prueba"
        };

        Assert.Equal(TipoApoyo.Beca, dto.TipoApoyo);
        Assert.Equal(1500000, dto.MontoSolicitado);
    }

    [Fact]
    public void CambiarEstadoDto_PropiedadesAsignables()
    {
        var id = Guid.NewGuid();
        var dto = new CambiarEstadoDto { SolicitudId = id, Accion = "aprobar", Observacion = "Cumple requisitos" };

        Assert.Equal(id, dto.SolicitudId);
        Assert.Equal("aprobar", dto.Accion);
        Assert.Equal("Cumple requisitos", dto.Observacion);
    }

    [Fact]
    public void AsignarAsesorDto_PropiedadesAsignables()
    {
        var solicitudId = Guid.NewGuid();
        var asesorId = Guid.NewGuid();
        var dto = new AsignarAsesorDto { SolicitudId = solicitudId, AsesorId = asesorId, Observacion = "Asignado" };

        Assert.Equal(solicitudId, dto.SolicitudId);
        Assert.Equal(asesorId, dto.AsesorId);
    }

    [Theory]
    [InlineData(10, 3, 4)]
    [InlineData(9, 3, 3)]
    [InlineData(1, 10, 1)]
    [InlineData(0, 10, 0)]
    public void PagedResultDto_TotalPages_CalculaCorrecto(int total, int pageSize, int expectedPages)
    {
        var dto = new PagedResultDto<string> { Total = total, PageSize = pageSize };

        Assert.Equal(expectedPages, dto.TotalPages);
    }
}
