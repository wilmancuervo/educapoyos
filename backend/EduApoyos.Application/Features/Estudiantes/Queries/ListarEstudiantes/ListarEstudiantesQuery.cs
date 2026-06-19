using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Estudiantes;
using MediatR;

namespace EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;

public record ListarEstudiantesQuery(int Page, int PageSize) : IRequest<PagedResultDto<EstudianteDto>>;
