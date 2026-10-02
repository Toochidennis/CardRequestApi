using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CardRequestApi.OpenApi;

internal sealed class ApiKeySecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["ApiKey"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "X-API-Key",
                In = ParameterLocation.Header,
                Description = "Enter the API key",
            },
        };

        foreach (
            var operation in document
                .Paths.Values.Where(path => path.Operations is not null)
                .SelectMany(path => path.Operations!)
        )
        {
            operation.Value.Security ??= [];

            operation.Value.Security.Add(
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("ApiKey", document)] = [],
                }
            );
        }

        return Task.CompletedTask;
    }
}
