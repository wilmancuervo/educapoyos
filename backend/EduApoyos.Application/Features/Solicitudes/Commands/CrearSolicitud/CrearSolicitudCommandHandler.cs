using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Commands.CrearSolicitud;

public class CrearSolicitudCommandHandler : IRequestHandler<CrearSolicitudCommand, Result<SolicitudDto>>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public CrearSolicitudCommandHandler(ISolicitudApoyoRepository solicitudRepository, IEstudianteRepository estudianteRepository)
    {
        _solicitudRepository = solicitudRepository;
        _estudianteRepository = estudianteRepository;
    }

    public async Task<Result<SolicitudDto>> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
    {
        var targetId = request.EstudianteUsuarioId ?? request.CallerUsuarioId;
        var estudiante = await _estudianteRepository.GetByUsuarioIdAsync(targetId);
        if (estudiante is null)
            return Result.Failure<SolicitudDto>(ApplicationErrors.Estudiante.NoEncontrado);

        var solicitud = new SolicitudApoyo(estudiante.Id, request.TipoApoyo, request.MontoSolicitado, request.Descripcion);

        await _solicitudRepository.AddAsync(solicitud);
        await _solicitudRepository.SaveChangesAsync();

        return Result.Success(SolicitudDto.FromEntity(solicitud));
    }
}
