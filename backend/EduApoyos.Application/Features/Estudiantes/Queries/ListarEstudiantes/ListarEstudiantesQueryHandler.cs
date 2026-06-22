using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;

public class ListarEstudiantesQueryHandler : IRequestHandler<ListarEstudiantesQuery, PagedResultDto<EstudianteDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarEstudiantesQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<PagedResultDto<EstudianteDto>> Handle(ListarEstudiantesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _usuarioRepository.GetEstudiantesPagedAsync(request.Page, request.PageSize);

        return new PagedResultDto<EstudianteDto>
        {
            Items = items.Select(EstudianteDto.FromUsuario),
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
