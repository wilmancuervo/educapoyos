using EduApoyos.Application.Common.Errors;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;

public class AsignarAsesorCommandHandler : IRequestHandler<AsignarAsesorCommand, Result>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public AsignarAsesorCommandHandler(ISolicitudApoyoRepository solicitudRepository, IUsuarioRepository usuarioRepository)
    {
        _solicitudRepository = solicitudRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result> Handle(AsignarAsesorCommand request, CancellationToken cancellationToken)
    {
        var asesor = await _usuarioRepository.GetByIdAsync(request.AsesorId);
        if (asesor is null)
            return Result.Failure(ApplicationErrors.Usuario.NoEncontrado);

        if (asesor.Rol != Rol.Asesor)
            return Result.Failure(ApplicationErrors.Usuario.NoEsAsesor);

        var solicitud = await _solicitudRepository.GetByIdWithHistorialAsync(request.SolicitudId);
        if (solicitud is null)
            return Result.Failure(ApplicationErrors.Solicitud.NoEncontrada);

        var estadoAnterior = solicitud.Estado;
        var resultado = solicitud.AsignarAsesor(request.AsesorId);
        if (resultado.IsFailure)
            return resultado;

        var historial = new HistorialEstado(solicitud.Id, request.UsuarioQueAsignaId, estadoAnterior, solicitud.Estado, request.Observacion);
        solicitud.Historial.Add(historial);

        _solicitudRepository.Update(solicitud);
        await _solicitudRepository.SaveChangesAsync();

        return Result.Success();
    }
}
