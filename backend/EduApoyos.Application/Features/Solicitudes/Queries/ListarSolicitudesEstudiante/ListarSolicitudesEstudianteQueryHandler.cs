using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;

public class ListarSolicitudesEstudianteQueryHandler : IRequestHandler<ListarSolicitudesEstudianteQuery, Result<IEnumerable<SolicitudDto>>>
{
    private readonly ISolicitudApoyoRepository _solicitudRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public ListarSolicitudesEstudianteQueryHandler(ISolicitudApoyoRepository solicitudRepository, IEstudianteRepository estudianteRepository)
    {
        _solicitudRepository = solicitudRepository;
        _estudianteRepository = estudianteRepository;
    }

    public async Task<Result<IEnumerable<SolicitudDto>>> Handle(ListarSolicitudesEstudianteQuery request, CancellationToken cancellationToken)
    {
        var estudiante = await _estudianteRepository.GetByUsuarioIdAsync(request.UsuarioId);
        if (estudiante is null)
            return Result.Failure<IEnumerable<SolicitudDto>>(ApplicationErrors.Estudiante.NoEncontrado);

        var solicitudes = await _solicitudRepository.GetByEstudianteIdAsync(estudiante.Id);

        return Result.Success(solicitudes.Select(SolicitudDto.FromEntity));
    }
}
