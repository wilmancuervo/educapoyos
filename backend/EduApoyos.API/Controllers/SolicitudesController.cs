using EduApoyos.API.Common;
using EduApoyos.Application.DTOs.Common;
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

/// <summary>
/// Gestión de solicitudes de apoyo económico (becas, créditos, subsidios).
/// </summary>
[ApiController]
[Route("api/solicitudes")]
[Authorize]
[Produces("application/json")]
public class SolicitudesController : AppController
{
    private readonly IMediator _mediator;

    public SolicitudesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todas las solicitudes con filtros opcionales y paginación. Solo Asesor.
    /// </summary>
    /// <param name="page">Número de página (default: 1).</param>
    /// <param name="pageSize">Registros por página (default: 10).</param>
    /// <param name="estado">Filtrar por estado (Pendiente, EnRevision, Aprobada, Rechazada).</param>
    /// <param name="tipo">Filtrar por tipo de apoyo (Beca, Credito, Subsidio).</param>
    /// <param name="desde">Fecha de inicio del rango.</param>
    /// <param name="hasta">Fecha de fin del rango.</param>
    [HttpGet]
    [Authorize(Roles = "Asesor")]
    [ProducesResponseType(typeof(PagedResultDto<SolicitudDto>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Crea una nueva solicitud de apoyo económico. Asesor o Estudiante.
    /// </summary>
    /// <param name="dto">Datos de la solicitud.</param>
    [HttpPost]
    [Authorize(Roles = "Asesor,Estudiante")]
    [ProducesResponseType(typeof(SolicitudDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearSolicitudDto dto)
    {
        var rol = ObtenerRol();
        var estudianteUsuarioId = rol == Rol.Asesor
            ? dto.EstudianteUsuarioId ?? Guid.Empty
            : ObtenerUsuarioId();

        if (estudianteUsuarioId == Guid.Empty)
            return IdentityUnauthorized();

        var resultado = await _mediator.Send(new CrearSolicitudCommand(
            estudianteUsuarioId, dto.TipoApoyo, dto.MontoSolicitado, dto.Descripcion));

        return Match(resultado,
            value => CreatedAtAction(nameof(ObtenerPorId), new { id = value.Id }, value),
            BadRequestError);
    }

    /// <summary>
    /// Obtiene el detalle de una solicitud con su historial de estados.
    /// </summary>
    /// <param name="id">ID de la solicitud.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SolicitudDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        var resultado = await _mediator.Send(new ObtenerSolicitudQuery(id, usuarioId, rol));

        return Match(resultado, Ok, NotFoundError);
    }

    /// <summary>
    /// Aprueba o rechaza una solicitud. Solo Asesor.
    /// </summary>
    /// <param name="id">ID de la solicitud.</param>
    /// <param name="dto">Acción (Aprobar / Rechazar) y observación opcional.</param>
    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "Asesor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoDto dto)
    {
        var asesorId = ObtenerUsuarioId();
        if (asesorId == Guid.Empty)
            return IdentityUnauthorized();

        var resultado = await _mediator.Send(new CambiarEstadoCommand(id, dto.Accion, asesorId, dto.Observacion));

        return Match(resultado, NoContent, BadRequestError);
    }

    /// <summary>
    /// Asigna un asesor responsable a una solicitud. Solo Asesor.
    /// </summary>
    /// <param name="id">ID de la solicitud.</param>
    /// <param name="dto">ID del asesor a asignar y observación opcional.</param>
    [HttpPost("{id}/asesor")]
    [Authorize(Roles = "Asesor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AsignarAsesor(Guid id, [FromBody] AsignarAsesorDto dto)
    {
        var usuarioQueAsigna = ObtenerUsuarioId();
        if (usuarioQueAsigna == Guid.Empty)
            return IdentityUnauthorized();

        var resultado = await _mediator.Send(new AsignarAsesorCommand(id, dto.AsesorId, usuarioQueAsigna, dto.Observacion));

        return Match(resultado, NoContent, BadRequestError);
    }

}
