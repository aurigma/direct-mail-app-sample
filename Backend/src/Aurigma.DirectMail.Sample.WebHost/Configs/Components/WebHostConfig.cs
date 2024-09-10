using System.Collections.Generic;
using Aurigma.DirectMail.Sample.WebHost.Configs.Common;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Aurigma.DirectMail.Sample.WebHost.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of component <see cref="DirectMail.Sample.WebApi"/>.
/// </summary>
public static class WebHostConfig
{
    private const string DatetimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"; // See "RFC 3339, section 5.6".

    /// <summary>
    /// Adds configuration for component <see cref="DirectMail.Sample.WebApi"/>.
    /// </summary>
    public static void AddWebHostConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
    {
        ConfigureWebApi(services, configuration);
    }

    private static void ConfigureWebApi(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = DatetimeFormat;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.AddSwaggerConfig(
            new SwaggerSettings
            {
                DocumentVersion = "v1",
                DocumentName = "v1",
                AppTitle = "Direct Mail API",
                XmlFilesPaths = new List<string> { "Aurigma.DirectMail.Sample.WebApi" },
            }
        );
    }
}
