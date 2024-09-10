using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Aurigma.DirectMail.Sample.WebHost.Helpers;

/// <summary>
/// Application configuration helper.
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// Returns the runtime environment name from the environment variable 'ASPNETCORE_ENVIRONMENT'.
    /// </summary>
    /// <remarks>
    /// 'ASPNETCORE_ENVIRONMENT' is an environment variable, which ASP.NET Core uses to identify the runtime environment.
    /// </remarks>
    public static string GetAspNetCoreEnvironmentName()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }

    /// <summary>
    /// Builds an application configuration based on appsettings files and environment variables.
    /// Used when the application builder is not yet available, for example, when launching the application.
    /// </summary>
    /// <param name="environmentName">Runtime environment name.</param>
    /// <returns>Application configuration.</returns>
    public static IConfiguration BuildAppConfiguration(string environmentName)
    {
        var confirugation = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{environmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables()
            .Build();

        var configurationString = confirugation.AsEnumerable().ToList();
        Console.WriteLine(JsonConvert.SerializeObject(configurationString));
        return confirugation;
    }
}
