using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Enums;
using MediatR;

namespace EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;

public record CrearEstudianteCommand(
    Guid UsuarioId,
    string NumeroDocumento,
    TipoDocumento TipoDocumento,
    string ProgramaAcademico,
    int Semestre) : IRequest<Result<EstudianteDto>>;
