using System;
using Aurigma.DirectMail.Sample.WebHost;
using Aurigma.DirectMail.Sample.WebHost.Helpers;
using Aurigma.DirectMail.Sample.WebHost.Providers;
using Microsoft.AspNetCore.Builder;

LoggerProvider.Configure(
    ConfigurationHelper.BuildAppConfiguration(ConfigurationHelper.GetAspNetCoreEnvironmentName())
);

var startupLogger = StartupLoggerProvider.GetLogger();

try
{
    startupLogger.Debug("Program: application configure started");

    var builder = WebApplication.CreateBuilder(args);
    builder.UseLoggerProvider();
    builder.ConfigureServices(startupLogger);

    startupLogger.Debug("Program: application build started");

    var app = builder.Build();
    app.Configure();
    app.ApplyMigrationOnStartup();

    startupLogger.Debug("Program: application has been successfully launched");

    app.Run();
}
catch (Exception ex)
{
    startupLogger.Error(ex, "Program: failed to launch the application");
    throw;
}
finally
{
    startupLogger.Dispose();
}
