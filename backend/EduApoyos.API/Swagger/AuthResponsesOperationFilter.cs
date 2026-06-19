using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EduApoyos.API.Swagger;

public class AuthResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any()
            || (context.MethodInfo.DeclaringType?.GetCustomAttributes<AuthorizeAttribute>(true).Any() ?? false);

        if (!hasAuthorize) return;

        operation.Responses.TryAdd("401", new OpenApiResponse
        {
            Description = "No autenticado. Se requiere un token JWT válido."
        });

        operation.Responses.TryAdd("403", new OpenApiResponse
        {
            Description = "Sin permiso. El rol del usuario no tiene acceso a este recurso."
        });
    }
}
