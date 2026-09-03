using System.Security.Cryptography;
using System.Text;

namespace CardRequestApi.Middleware;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeader = "X-API-Key";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Allow Swagger/OpenAPI documentation
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(
            ApiKeyHeader,
            out var providedKey))
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "API key is required"
            });

            return;
        }

        var configuredKey = _configuration["ApiKey"];

        if (string.IsNullOrWhiteSpace(configuredKey) ||
            !KeysMatch(providedKey.ToString(), configuredKey))
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Invalid API key"
            });

            return;
        }

        await _next(context);
    }

    private static bool KeysMatch(
        string providedKey,
        string configuredKey)
    {
        var providedBytes =
            Encoding.UTF8.GetBytes(providedKey);

        var configuredBytes =
            Encoding.UTF8.GetBytes(configuredKey);

        return CryptographicOperations.FixedTimeEquals(
            providedBytes,
            configuredBytes
        );
    }
}