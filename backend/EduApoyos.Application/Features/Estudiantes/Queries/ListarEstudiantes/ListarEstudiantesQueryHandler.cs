using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;

public class ListarEstudiantesQueryHandler : IRequestHandler<ListarEstudiantesQuery, PagedResultDto<EstudianteDto>>
{
    private readonly IEstudianteRepository _estudianteRepository;

    public ListarEstudiantesQueryHandler(IEstudianteRepository estudianteRepository)
    {
        _estudianteRepository = estudianteRepository;
    }

    public async Task<PagedResultDto<EstudianteDto>> Handle(ListarEstudiantesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _estudianteRepository.GetPagedAsync(request.Page, request.PageSize);

        var dtos = items.Select(EstudianteDto.FromEntity);

        return new PagedResultDto<EstudianteDto>
        {
            Items = dtos,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
