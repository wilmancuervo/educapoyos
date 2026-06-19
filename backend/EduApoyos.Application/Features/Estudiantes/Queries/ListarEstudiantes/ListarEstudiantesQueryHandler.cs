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

        var dtos = items.Select(e => new EstudianteDto
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            NombreCompleto = e.Usuario.NombreCompleto,
            Email = e.Usuario.Email,
            NumeroDocumento = e.NumeroDocumento,
            TipoDocumento = e.TipoDocumento.ToString(),
            ProgramaAcademico = e.ProgramaAcademico,
            Semestre = e.Semestre
        });

        return new PagedResultDto<EstudianteDto>
        {
            Items = dtos,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
