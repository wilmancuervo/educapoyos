using EduApoyos.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyos.API.Common;

public abstract class AppController : ControllerBase
{
    protected IActionResult Match<T>(Result<T> result, Func<T, IActionResult> onSuccess, Func<Error, IActionResult> onFailure)
        => result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);

    protected IActionResult Match(Result result, Func<IActionResult> onSuccess, Func<Error, IActionResult> onFailure)
        => result.IsSuccess ? onSuccess() : onFailure(result.Error);

    protected IActionResult BadRequestError(Error error) =>
        new ObjectResult(ApiErrorResponse.BadRequest(error)) { StatusCode = StatusCodes.Status400BadRequest };

    protected IActionResult NotFoundError(Error error) =>
        new ObjectResult(ApiErrorResponse.NotFound(error)) { StatusCode = StatusCodes.Status404NotFound };

    protected IActionResult ConflictError(Error error) =>
        new ObjectResult(ApiErrorResponse.Conflict(error)) { StatusCode = StatusCodes.Status409Conflict };

    protected IActionResult UnauthorizedError(Error error) =>
        new ObjectResult(ApiErrorResponse.Unauthorized(error.Code, error.Message)) { StatusCode = StatusCodes.Status401Unauthorized };

    protected IActionResult IdentityUnauthorized() =>
        new ObjectResult(ApiErrorResponse.Unauthorized()) { StatusCode = StatusCodes.Status401Unauthorized };
}
