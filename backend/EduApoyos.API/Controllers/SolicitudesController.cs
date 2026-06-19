using EduApoyos.Application.DTOs.Solicitudes;
using EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;
using EduApoyos.Application.Features.Solicitudes.Commands.CambiarEstado;
using EduApoyos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using EduApoyos.Application.Features.Solicitudes.Queries.ListarSolicitudes;
using EduApoyos.Application.Features.Solicitudes.Queries.ObtenerSolicitud;
using EduApoyos.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Controllers;

[ApiController]
[Route("api/solicitudes")]
[Authorize]
public class SolicitudesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SolicitudesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Asesor")]
    public async Task<IActionResult> Listar(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] EstadoSolicitud? estado = null,
        [FromQuery] TipoApoyo? tipo = null,
        [FromQuery] DateTime? desde = null,
        [FromQuery] DateTime? hasta = null)
    {
        var resultado = await _mediator.Send(new ListarSolicitudesQuery(page, pageSize, estado, tipo, desde, hasta));
        return Ok(resultado);
    }

    [HttpPost]
    [Authorize(Roles = "Asesor,Estudiante")]
    public async Task<IActionResult> Crear([FromBody] CrearSolicitudDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId == Guid.Empty)
            return Unauthorized();

        var resultado = await _mediator.Send(new CrearSolicitudCommand(
            usuarioId, dto.TipoApoyo, dto.MontoSolicitado, dto.Descripcion));

        if (resultado.IsFailure)
            return BadRequest(new { resultado.Error.Code, resultado.Error.Message });

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Value.Id }, resultado.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        var resultado = await _mediator.Send(new ObtenerSolicitudQuery(id, usuarioId, rol));

        if (resultado.IsFailure)
            return NotFound(new { resultado.Error.Code, resultado.Error.Message });

        return Ok(resultado.Value);
    }

    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "Asesor")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoDto dto)
    {
        var asesorId = ObtenerUsuarioId();
        if (asesorId == Guid.Empty)
            return Unauthorized();

        var resultado = await _mediator.Send(new CambiarEstadoCommand(id, dto.Accion, asesorId, dto.Observacion));

        if (resultado.IsFailure)
            return BadRequest(new { resultado.Error.Code, resultado.Error.Message });

        return NoContent();
    }

    [HttpPost("{id}/asesor")]
    [Authorize(Roles = "Asesor")]
    public async Task<IActionResult> AsignarAsesor(Guid id, [FromBody] AsignarAsesorDto dto)
    {
        var usuarioQueAsigna = ObtenerUsuarioId();
        if (usuarioQueAsigna == Guid.Empty)
            return Unauthorized();

        var resultado = await _mediator.Send(new AsignarAsesorCommand(id, dto.AsesorId, usuarioQueAsigna, dto.Observacion));

        if (resultado.IsFailure)
            return BadRequest(new { resultado.Error.Code, resultado.Error.Message });

        return NoContent();
    }

    private Guid ObtenerUsuarioId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                 ?? User.FindFirst("sub");
        return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }

    private Rol ObtenerRol()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);
        return claim is not null && Enum.TryParse<Rol>(claim.Value, out var rol) ? rol : Rol.Estudiante;
    }
}
