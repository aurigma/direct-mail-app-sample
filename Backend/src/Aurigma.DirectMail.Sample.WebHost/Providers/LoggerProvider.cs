using Aurigma.DirectMail.Sample.WebHost.Configs.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Aurigma.DirectMail.Sample.WebHost.Providers;

/// <summary>
///  Logger provider for working with <see cref="Microsoft.Extensions.Logging.ILogger{TCategoryName}"/> objects.
/// </summary>
public static class LoggerProvider
{
    private static bool _isConfigured;

    public static bool IsConfigured() => _isConfigured;

    public static void Configure(IConfiguration configuration)
    {
        NLogConfig.AddNLogConfig(configuration);

        _isConfigured = true;
    }

    public static void UseLoggerProvider(this WebApplicationBuilder builder)
    {
        builder.UseNLogAsLoggerProvider();
    }
}
