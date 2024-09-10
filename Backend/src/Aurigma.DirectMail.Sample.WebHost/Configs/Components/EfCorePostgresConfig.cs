using System;
using Aurigma.DirectMail.Sample.DAL.Postgres.Context;
using Aurigma.DirectMail.Sample.WebHost.Configs.Common;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of component <see cref="DAL.Postgres"/>
/// </summary>
public static class EfCorePostgresConfig
{
    /// <summary>
    /// Adds configuration for component <see cref="DAL.Postgres"/>.
    /// </summary>
    public static void AddEfCorePostgresConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
    {
        services.AddNpgsqlServerConfig<PostgresDataContext>(configuration, logger);
    }

    /// <summary>
    /// Applies any pending migrations for the context to the MS SQL database.
    /// Will create the database if it does not already exist.
    /// </summary>
    /// <exception cref="Exception">The <see cref="PostgresDataContext"/> is not configured.</exception>
    public static void ApplyPostgresMigration(
        this IApplicationBuilder app,
        IConfiguration configuration
    )
    {
        app.ApplyPostgresMigration<PostgresDataContext>(configuration);
    }
}
