using Microsoft.Extensions.Configuration;

namespace Aurigma.DirectMail.Sample.WebHost.Extensions;

/// <summary>
/// Application configuration extensions.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Indicates if the configuration section exists.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration section.</param>
    /// <returns><c>true</c> if the configuration section exists; <c>false</c> otherwise.</returns>
    public static bool IsSectionExists(this IConfiguration configuration, string path)
    {
        return configuration.GetSection(path).Exists();
    }

    /// <summary>
    /// Indicates if the configuration section does not exist.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration section.</param>
    /// <returns><c>true</c> if the configuration section does not exist; <c>false</c> otherwise.</returns>
    public static bool IsSectionNotExists(this IConfiguration configuration, string path)
    {
        return !configuration.IsSectionExists(path);
    }

    /// <summary>
    /// Casts the configuration section to the specified class type.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration section.</param>
    /// <typeparam name="T">Object type.</typeparam>
    /// <returns>Object of the specified type if the configuration section exists; <c>null</c> otherwise.</returns>
    public static T CastSection<T>(this IConfiguration configuration, string path)
        where T : class
    {
        return configuration.GetSection(path).Get<T>();
    }

    /// <summary>
    /// Indicates if the configuration parameter exists.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration parameter.</param>
    /// <returns>
    /// <c>true</c> if the configuration parameter exists and is not equal to null or an empty string;
    /// <c>false</c> otherwise.
    /// </returns>
    public static bool IsParameterExists(this IConfiguration configuration, string path)
    {
        return !string.IsNullOrEmpty(configuration.GetParam(path));
    }

    /// <summary>
    /// Indicates if the configuration parameter does not exist.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration parameter.</param>
    /// <returns>
    /// <c>true</c> if the configuration parameter does not exist or is equal to null or an empty string;
    /// <c>false</c> otherwise.
    /// </returns>
    public static bool IsParameterNotExists(this IConfiguration configuration, string path)
    {
        return !configuration.IsParameterExists(path);
    }

    /// <summary>
    /// Checks for the presence of the parameter and gets it from the configuration.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration parameter.</param>
    /// <param name="value">Parameter value obtained from the configuration.</param>
    /// <returns>
    /// <c>true</c> if the configuration parameter exists and is not equal to null or an empty string;
    /// <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetParam(this IConfiguration configuration, string path, out string value)
    {
        value = configuration.GetParam(path);

        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Returns the parameter value from the configuration.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="path">Path to the configuration parameter.</param>
    /// <returns>Parameter value in <c>string</c> format if the configuration parameter exists; <c>null</c> otherwise.</returns>
    public static string GetParam(this IConfiguration configuration, string path)
    {
        return configuration[path];
    }
}
