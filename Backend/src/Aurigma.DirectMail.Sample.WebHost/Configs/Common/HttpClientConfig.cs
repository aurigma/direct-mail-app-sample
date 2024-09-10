using Aurigma.DirectMail.Sample.WebHost.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// HTTP client configuration.
/// </summary>
public static class HttpClientConfig
{
    /// <summary>
    /// Adds a configuration for HTTP client.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddHttpConfig(this IServiceCollection services)
    {
        const string defaultClientName = "default";

        services.AddHttpClient(defaultClientName);
        services.AddSingleton<HttpClientProvider>();
        services.AddTransient(sp =>
            sp.GetRequiredService<HttpClientProvider>().GetHttpClient(defaultClientName)
        );
    }
}
