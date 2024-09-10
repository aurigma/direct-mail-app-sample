using System.Collections.Generic;
using System.Linq;
using Aurigma.DirectMail.Sample.WebHost.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// CORS configuration.
/// </summary>
public static class CorsConfig
{
    private const string AllowedCorsOriginsDefaultConfigParam = "Cors:AllowedCorsOriginsDefault";
    private const string AllowedCorsOriginsPublicConfigParam = "Cors:AllowedCorsOriginsPublic";

    /// <summary>
    /// Adds a configuration for CORS policy.
    /// </summary>
    /// <param name="services">Used to register application services.</param>
    /// <param name="configuration">Application configuration.</param>
    public static void AddCorsPolicyConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(x => x.AddDefaultPolicy(configuration));
            options.AddPolicy("Public", x => x.AddPublicPolicy(configuration));
            options.AddPolicy("AllowAny", x => x.AddAllowAnyPolicy());
        });
    }

    #region Private logic

    private static void AddDefaultPolicy(
        this CorsPolicyBuilder builder,
        IConfiguration configuration
    )
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();

        var allowedCorsOrigins = GetCorsOriginsForDefaultPolicy(configuration).ToArray();

        builder = allowedCorsOrigins.Any(x => x == "*")
            ? builder.AllowAnyOrigin()
            : builder.WithOrigins(allowedCorsOrigins.ToArray());
    }

    private static void AddPublicPolicy(
        this CorsPolicyBuilder builder,
        IConfiguration configuration
    )
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();

        var allowedCorsOrigins = GetCorsOriginsForPublicPolicy(configuration).ToArray();

        builder = allowedCorsOrigins.Any(x => x == "*")
            ? builder.AllowAnyOrigin()
            : builder.WithOrigins(allowedCorsOrigins.ToArray());
    }

    private static void AddAllowAnyPolicy(this CorsPolicyBuilder builder)
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }

    private static IEnumerable<string> GetCorsOriginsForDefaultPolicy(IConfiguration configuration)
    {
        var allowedCorsOrigins =
            configuration.GetParam(AllowedCorsOriginsDefaultConfigParam) ?? "*";
        return allowedCorsOrigins.Split(',');
    }

    private static IEnumerable<string> GetCorsOriginsForPublicPolicy(IConfiguration configuration)
    {
        var allowedCorsOrigins = configuration.GetParam(AllowedCorsOriginsPublicConfigParam) ?? "*";
        return allowedCorsOrigins.Split(',');
    }

    #endregion
}
