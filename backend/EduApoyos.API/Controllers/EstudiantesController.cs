using EduApoyos.Application.DTOs.Estudiantes;
using EduApoyos.Application.Features.Estudiantes.Commands.CrearEstudiante;
using EduApoyos.Application.Features.Estudiantes.Queries.ListarEstudiantes;
using EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudesEstudiante;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Authorize]
public class EstudiantesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstudiantesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Asesor")]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await _mediator.Send(new ListarEstudiantesQuery(page, pageSize));
        return Ok(resultado);
    }

    [HttpPost]
    [Authorize(Roles = "Asesor")]
    public async Task<IActionResult> Crear([FromBody] CrearEstudianteDto dto)
    {
        var resultado = await _mediator.Send(new CrearEstudianteCommand(
            dto.UsuarioId, dto.NumeroDocumento, dto.TipoDocumento, dto.ProgramaAcademico, dto.Semestre));

        if (resultado.IsFailure)
            return BadRequest(new { resultado.Error.Code, resultado.Error.Message });

        return CreatedAtAction(nameof(Listar), resultado.Value);
    }

    [HttpGet("{id}/solicitudes")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> ListarSolicitudes(Guid id)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId == Guid.Empty)
            return Unauthorized();

        var resultado = await _mediator.Send(new ListarSolicitudesEstudianteQuery(usuarioId));

        if (resultado.IsFailure)
            return NotFound(new { resultado.Error.Code, resultado.Error.Message });

        return Ok(resultado.Value);
    }

    private Guid ObtenerUsuarioId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                 ?? User.FindFirst("sub");
        return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }
}
