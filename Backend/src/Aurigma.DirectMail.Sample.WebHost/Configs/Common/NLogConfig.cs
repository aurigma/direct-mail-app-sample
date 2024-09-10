using Aurigma.DirectMail.Sample.WebHost.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Web;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// NLog configuration.
/// </summary>
public static class NLogConfig
{
    private const string NLogConfigParam = "NLog:ConfigFile";
    private const string NLogDefaultConfigFileName = "NLog.config";

    /// <summary>
    /// Adds a configuration for NLog framework.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    public static void AddNLogConfig(IConfiguration configuration)
    {
        SetLogManagerConfiguration(configuration);
    }

    public static void UseNLogAsLoggerProvider(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
    }

    #region Private logic

    private static void SetLogManagerConfiguration(IConfiguration configuration)
    {
        var nlogConfigurationFileName = GetNLogConfigFileName(configuration);
        LogManager.Configuration = new XmlLoggingConfiguration(nlogConfigurationFileName);
    }

    private static string GetNLogConfigFileName(IConfiguration configuration)
    {
        var nlogConfigFileName = configuration.GetParam(NLogConfigParam);

        return string.IsNullOrEmpty(nlogConfigFileName)
            ? NLogDefaultConfigFileName
            : nlogConfigFileName;
    }

    #endregion Private logic
}
