using EduApoyos.Application.Common.Errors;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.CambiarEstado;

public class CambiarEstadoCommandHandler : IRequestHandler<CambiarEstadoCommand, Result>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;

    public CambiarEstadoCommandHandler(ISolicitudApoyoRepository solicitudRepository)
    {
        _solicitudRepository = solicitudRepository;
    }

    public async Task<Result> Handle(CambiarEstadoCommand request, CancellationToken cancellationToken)
    {
        var solicitud = await _solicitudRepository.GetByIdWithHistorialAsync(request.SolicitudId);
        if (solicitud is null)
            return Result.Failure(ApplicationErrors.Solicitud.NoEncontrada);

        var estadoAnterior = solicitud.Estado;

        var resultado = request.Accion.ToLower() switch
        {
            "aprobar" => solicitud.Aprobar(),
            "rechazar" => solicitud.Rechazar(),
            _ => Result.Failure(ApplicationErrors.Solicitud.AccionInvalida)
        };

        if (resultado.IsFailure)
            return resultado;

        var historial = new HistorialEstado(solicitud.Id, request.AsesorId, estadoAnterior, solicitud.Estado, request.Observacion);
        solicitud.Historial.Add(historial);

        _solicitudRepository.Update(solicitud);
        await _solicitudRepository.SaveChangesAsync();

        return Result.Success();
    }
}
