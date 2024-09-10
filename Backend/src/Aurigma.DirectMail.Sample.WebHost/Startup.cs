using Aurigma.DirectMail.Sample.WebHost.Configs.Common;
using Aurigma.DirectMail.Sample.WebHost.Configs.Components;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Aurigma.DirectMail.Sample.WebHost;

/// <summary>
/// Application launch configuration.
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configures all necessary services.
    /// </summary>
    /// <param name="builder">A builder for web applications and services.</param>
    /// <param name="logger">The logger.</param>
    public static void ConfigureServices(this WebApplicationBuilder builder, IStartupLogger logger)
    {
        var configuration = builder.Configuration;

        // Configure application components
        builder.Services.AddAppServicesConfig(configuration, logger);
        builder.Services.AddDomainServicesConfig();
        builder.Services.AddEfCoreConfig();
        builder.Services.AddEfCorePostgresConfig(configuration, logger);
        builder.Services.AddFileSystemConfig();
        builder.Services.AddWebHostConfig(configuration, logger);

        // Configure common components
        builder.Services.AddHttpConfig();
        builder.Services.AddCorsPolicyConfig(configuration);
        builder.Services.AddHttpLoggingConfig(configuration, logger);
        builder.Services.AddAutoMapperConfig();
        builder.Services.AddCustomersCanvasApiClientConfig(configuration, logger);
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The web application used to configure the HTTP pipeline, and routes.</param>
    public static void Configure(this WebApplication app)
    {
        app.UseExceptionHandler(x => x.UseCustomExceptionHandler(app.Environment));
        app.UseHealthChecks();
        app.UseHttpLogging();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Direct Mail sample API V1");
            options.DocExpansion(DocExpansion.List);
            options.DefaultModelExpandDepth(1);
        });
    }

    /// <summary>
    /// Applies migration on startup application.
    /// </summary>
    /// <param name="app">The web application used to configure the HTTP pipeline, and routes.</param>
    public static void ApplyMigrationOnStartup(this WebApplication app)
    {
        app.ApplyPostgresMigration(app.Configuration);
    }
}
