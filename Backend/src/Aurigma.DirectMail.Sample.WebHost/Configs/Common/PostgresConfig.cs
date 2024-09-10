using System;
using Aurigma.DirectMail.Sample.WebHost.Extensions;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Aurigma.DirectMail.Sample.WebHost.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// PostgreSQL database connection configuration.
/// </summary>
public static class PostgresConfig
{
    private const string ConnectionString = "DefaultConnection";
    private const string EfCoreConfigSection = "EntityFramework";

    /// <summary>
    /// Adds a configuration to connect to the Postgres database.
    /// </summary>
    public static void AddNpgsqlServerConfig<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(ConnectionString);

        if (string.IsNullOrEmpty(connectionString))
        {
            logger.Warn($"Connection string '{ConnectionString}' is not specified");
            return;
        }

        var efCoreSettings = configuration.CastSection<EntityFrameworkSettings>(
            EfCoreConfigSection
        );

        services.AddScoped<DbContext, TContext>();

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            if (efCoreSettings?.EnableSensitiveDataLogging == true)
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }

    /// <summary>
    /// Applies database migrations.
    /// </summary>
    public static void ApplyPostgresMigration<TContext>(
        this IApplicationBuilder app,
        IConfiguration configuration
    )
        where TContext : DbContext
    {
        if (string.IsNullOrEmpty(configuration.GetConnectionString(ConnectionString)))
        {
            return;
        }

        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<TContext>();

        if (dbContext == null)
        {
            throw new Exception($"The {nameof(TContext)} is not configured");
        }

        var efCoreSettings = configuration.CastSection<EntityFrameworkSettings>(
            EfCoreConfigSection
        );
        if (efCoreSettings?.ApplyMigrationsOnStartup == true)
        {
            dbContext.Database.Migrate();
        }
    }
}
