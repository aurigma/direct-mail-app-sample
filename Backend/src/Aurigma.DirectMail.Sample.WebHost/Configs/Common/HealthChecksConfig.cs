using Aurigma.DirectMail.Sample.WebHost.Configs.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// Health checks configuration.
/// </summary>
public static class HealthChecksConfig
{
    private const string HealthCheckPath = "/health";

    /// <summary>
    /// Adds a middleware that provides health check status.
    /// </summary>
    /// <remarks>
    /// Also need to add the use of the <see cref="HealthCheckServiceCollectionExtensions.AddHealthChecks"/> method.
    /// </remarks>
    public static void UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks(HealthCheckPath, GetHealthCheckOptions());
    }

    private static HealthCheckOptions GetHealthCheckOptions()
    {
        return new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status202Accepted,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
        };
    }
}
