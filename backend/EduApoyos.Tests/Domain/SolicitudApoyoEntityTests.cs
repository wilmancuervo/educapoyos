using EduApoyos.Domain.Common.Errors;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Tests.Entities;

public class SolicitudApoyoEntityTests
{
    private static SolicitudApoyo BuildSolicitud() =>
        new(Guid.NewGuid(), TipoApoyo.Beca, 1_500_000, "Test");

    [Fact]
    public void PerteneceA_MismoEstudiante_RetornaTrue()
    {
        var estudianteId = Guid.NewGuid();
        var solicitud = new SolicitudApoyo(estudianteId, TipoApoyo.Beca, 1_000_000, "Test");

        Assert.True(solicitud.PerteneceA(estudianteId));
    }

    [Fact]
    public void PerteneceA_OtroEstudiante_RetornaFalse()
    {
        var solicitud = BuildSolicitud();

        Assert.False(solicitud.PerteneceA(Guid.NewGuid()));
    }

    [Fact]
    public void AsignarAsesor_AsesorIdVacio_RetornaError()
    {
        var solicitud = BuildSolicitud();

        var result = solicitud.AsignarAsesor(Guid.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Solicitud.AsesorInvalido.Code, result.Error.Code);
    }

    [Fact]
    public void AsignarAsesor_EstadoNoEsPendiente_RetornaError()
    {
        var solicitud = BuildSolicitud();
        solicitud.AsignarAsesor(Guid.NewGuid()); // pasa a EnRevision

        var result = solicitud.AsignarAsesor(Guid.NewGuid()); // intento segundo asesor

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Solicitud.EstadoInvalidoParaAsignar.Code, result.Error.Code);
    }

    [Fact]
    public void Aprobar_EstadoNoPendiente_RetornaError()
    {
        var solicitud = BuildSolicitud(); // estado Pendiente

        var result = solicitud.Aprobar();

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Solicitud.EstadoInvalidoParaAprobar.Code, result.Error.Code);
    }

    [Fact]
    public void Rechazar_EstadoNoPendiente_RetornaError()
    {
        var solicitud = BuildSolicitud(); // estado Pendiente

        var result = solicitud.Rechazar();

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Solicitud.EstadoInvalidoParaRechazar.Code, result.Error.Code);
    }
}
