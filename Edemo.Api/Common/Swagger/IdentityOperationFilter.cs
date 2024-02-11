using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Edemo.Api.Common.Swagger;

public class IdentityOperationFilter : IOperationFilter
{
    private const string IdentityControllerName = "Edemo.Api";
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor.RouteValues.TryGetValue("controller", out var controllerName))
        {
            if (controllerName?.Equals(IdentityControllerName, StringComparison.OrdinalIgnoreCase) == true)
            {
                operation.Tags.Clear();
                operation.Tags.Add(new OpenApiTag { Name = "Identity" });
            }
        }
    }
}