using EduApoyos.API.Common;
using EduApoyos.Application.DTOs.Common;
using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;
using EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;
using EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Controllers;

/// <summary>
/// Gestión de estudiantes y consulta de sus solicitudes.
/// </summary>
[ApiController]
[Route("api/estudiantes")]
[Authorize]
[Produces("application/json")]
public class EstudiantesController : AppController
{
    private readonly IMediator _mediator;

    public EstudiantesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todos los estudiantes registrados con paginación. Solo Asesor.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Asesor")]
    [ProducesResponseType(typeof(PagedResultDto<EstudianteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await _mediator.Send(new ListarEstudiantesQuery(page, pageSize));
        return Ok(resultado);
    }

    /// <summary>
    /// Crea el perfil de un estudiante vinculado a un usuario existente. Solo Asesor.
    /// </summary>
    /// <param name="dto">Datos del perfil del estudiante.</param>
    [HttpPost]
    [Authorize(Roles = "Asesor")]
    [ProducesResponseType(typeof(EstudianteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearEstudianteDto dto)
    {
        var resultado = await _mediator.Send(new CrearEstudianteCommand(
            dto.UsuarioId, dto.NumeroDocumento, dto.TipoDocumento, dto.ProgramaAcademico, dto.Semestre));

        return Match(resultado, value => CreatedAtAction(nameof(Listar), value), BadRequestError);
    }

    /// <summary>
    /// Devuelve las solicitudes del estudiante autenticado. Solo Estudiante.
    /// </summary>
    /// <param name="id">ID del estudiante (debe coincidir con el usuario autenticado).</param>
    [HttpGet("{id}/solicitudes")]
    [Authorize(Roles = "Estudiante")]
    [ProducesResponseType(typeof(IEnumerable<SolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarSolicitudes(Guid id)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId == Guid.Empty)
            return IdentityUnauthorized();

        var resultado = await _mediator.Send(new ListarSolicitudesEstudianteQuery(usuarioId));

        return Match(resultado, Ok, NotFoundError);
    }

}
