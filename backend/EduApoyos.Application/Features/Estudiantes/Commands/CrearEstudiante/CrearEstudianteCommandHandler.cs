using EduApoyos.Application.Common.Errors;
using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Domain.Common;
using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Interfaces;
using MediatR;

namespace EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;

public class CrearEstudianteCommandHandler : IRequestHandler<CrearEstudianteCommand, Result<EstudianteDto>>
{
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public CrearEstudianteCommandHandler(IEstudianteRepository estudianteRepository, IUsuarioRepository usuarioRepository)
    {
        _estudianteRepository = estudianteRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<EstudianteDto>> Handle(CrearEstudianteCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            return Result.Failure<EstudianteDto>(ApplicationErrors.Usuario.NoEncontrado);

        var existente = await _estudianteRepository.GetByUsuarioIdAsync(request.UsuarioId);
        if (existente is not null)
            return Result.Failure<EstudianteDto>(ApplicationErrors.Estudiante.YaExiste);

        var estudiante = new Estudiante(request.UsuarioId, request.NumeroDocumento, request.TipoDocumento, request.ProgramaAcademico, request.Semestre);

        await _estudianteRepository.AddAsync(estudiante);
        await _estudianteRepository.SaveChangesAsync();

        return Result.Success(EstudianteDto.FromEntity(estudiante));
    }
}
