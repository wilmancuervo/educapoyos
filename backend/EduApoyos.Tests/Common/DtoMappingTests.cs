using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Tests.TestHelpers;

namespace EduApoyos.Tests.Common;

public class DtoMappingTests
{
    // ── EstudianteDto ────────────────────────────────────────────────────────

    [Fact]
    public void EstudianteDto_FromUsuario_SinPerfil_CamposNullables()
    {
        var usuario = new Usuario("Ana García", "ana@test.com", "hash", Rol.Estudiante);

        var dto = EstudianteDto.FromUsuario(usuario);

        Assert.Equal(usuario.Id, dto.UsuarioId);
        Assert.Equal("Ana García", dto.NombreCompleto);
        Assert.Null(dto.EstudianteId);
        Assert.Null(dto.NumeroDocumento);
        Assert.Null(dto.TipoDocumento);
        Assert.Null(dto.ProgramaAcademico);
        Assert.Null(dto.Semestre);
    }

    [Fact]
    public void EstudianteDto_FromUsuario_ConPerfil_MappeaCompleto()
    {
        var usuario = new Usuario("Pedro Rojas", "pedro@test.com", "hash", Rol.Estudiante);
        var estudiante = new Estudiante(usuario.Id, "1012345678", TipoDocumento.CedulaCiudadania, "Ingeniería de Sistemas", 5);
        typeof(Usuario).GetProperty("Estudiante")!.SetValue(usuario, estudiante);

        var dto = EstudianteDto.FromUsuario(usuario);

        Assert.Equal(estudiante.Id, dto.EstudianteId);
        Assert.Equal("1012345678", dto.NumeroDocumento);
        Assert.Equal(TipoDocumento.CedulaCiudadania.ToString(), dto.TipoDocumento);
        Assert.Equal("Ingeniería de Sistemas", dto.ProgramaAcademico);
        Assert.Equal(5, dto.Semestre);
    }

    [Fact]
    public void EstudianteDto_FromEntity_MappeaCompleto()
    {
        var estudiante = DomainBuilders.BuildEstudiante();

        var dto = EstudianteDto.FromEntity(estudiante);

        Assert.Equal(estudiante.UsuarioId, dto.UsuarioId);
        Assert.NotNull(dto.NombreCompleto);
        Assert.NotNull(dto.Email);
        Assert.Equal(estudiante.Id, dto.EstudianteId);
        Assert.Equal(estudiante.NumeroDocumento, dto.NumeroDocumento);
    }

    // ── SolicitudDto ─────────────────────────────────────────────────────────

    [Fact]
    public void SolicitudDto_FromEntity_SinAsesor_NombreAsesorNull()
    {
        var solicitud = DomainBuilders.BuildSolicitud();

        var dto = SolicitudDto.FromEntity(solicitud);

        Assert.Equal(solicitud.Id, dto.Id);
        Assert.Equal(solicitud.TipoApoyo.ToString(), dto.TipoApoyo);
        Assert.Equal(solicitud.Estado.ToString(), dto.Estado);
        Assert.Null(dto.NombreAsesor);
    }

    [Fact]
    public void SolicitudDto_FromEntity_ConAsesor_NombreAsesorMapeado()
    {
        var solicitud = DomainBuilders.BuildSolicitudConAsesor();

        var dto = SolicitudDto.FromEntity(solicitud);

        Assert.NotNull(dto.NombreAsesor);
        Assert.Equal(solicitud.Asesor!.NombreCompleto, dto.NombreAsesor);
    }
}
