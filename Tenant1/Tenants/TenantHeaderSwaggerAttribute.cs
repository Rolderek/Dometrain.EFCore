using Microsoft.OpenApi.Models;

namespace Tenant1.Tenants
{
    public class TenantHeaderSwaggerAttribute : InvalidOperationException
    {
        public void Apply(OpenApiOperation operation, OpenApiParameter parameter)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Tenant",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }

    }
}
