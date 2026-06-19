using EduApoyos.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Common;

public sealed class ApiErrorResponse : ProblemDetails
{
    private const string RfcType = "about:blank";

    private ApiErrorResponse() { }

    public static ApiErrorResponse BadRequest(Error error) => new()
    {
        Type = RfcType,
        Title = error.Code,
        Detail = error.Message,
        Status = StatusCodes.Status400BadRequest
    };

    public static ApiErrorResponse NotFound(Error error) => new()
    {
        Type = RfcType,
        Title = error.Code,
        Detail = error.Message,
        Status = StatusCodes.Status404NotFound
    };

    public static ApiErrorResponse Conflict(Error error) => new()
    {
        Type = RfcType,
        Title = error.Code,
        Detail = error.Message,
        Status = StatusCodes.Status409Conflict
    };

    public static ApiErrorResponse Unauthorized(
        string code = "NO_AUTENTICADO",
        string detail = "No se pudo identificar el usuario autenticado.") => new()
    {
        Type = RfcType,
        Title = code,
        Detail = detail,
        Status = StatusCodes.Status401Unauthorized
    };

    public static ApiErrorResponse InternalServerError() => new()
    {
        Type = RfcType,
        Title = "ERROR_INTERNO",
        Detail = "Ocurrió un error inesperado. Intente de nuevo más tarde.",
        Status = StatusCodes.Status500InternalServerError
    };
}
