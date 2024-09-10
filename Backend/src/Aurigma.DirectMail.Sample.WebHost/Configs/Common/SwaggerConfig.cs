using System;
using Aurigma.DirectMail.Sample.WebHost.Filters;
using Aurigma.DirectMail.Sample.WebHost.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// Swagger configuration.
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Adds a configuration for Swagger.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="swaggerSettings"></param>
    public static void AddSwaggerConfig(
        this IServiceCollection services,
        SwaggerSettings swaggerSettings
    )
    {
        services.AddSwaggerGenNewtonsoftSupport();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                swaggerSettings.DocumentName,
                new OpenApiInfo
                {
                    Title = swaggerSettings.AppTitle,
                    Version = swaggerSettings.DocumentVersion,
                }
            );

            options.OperationFilter<ResponseHeadersOperationFilter>();

            swaggerSettings.XmlFilesPaths.ForEach(filePath =>
                options.IncludeXmlComments(GetXmlCommentsPath(filePath), true)
            );
        });
    }

    private static string GetXmlCommentsPath(string moduleName)
    {
        return $@"{AppDomain.CurrentDomain.BaseDirectory}/{moduleName}.xml";
    }
}
