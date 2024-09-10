using System.IO;
using System.Reflection;

namespace Aurigma.DirectMail.Sample.WebApi.Extensions;

/// <summary>
/// Extensions for working with the assembly and its attributes.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns the version of the assembly.
    /// </summary>
    public static string GetVersion(this Assembly assembly)
    {
        var version = assembly.GetName().Version;

        return version != null
            ? $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}"
            : string.Empty;
    }

    /// <summary>
    /// Returns the date (UTC) of the assembly.
    /// </summary>
    public static string GetAssemblyDate(this Assembly assembly)
    {
        return File.GetLastWriteTimeUtc(assembly.Location).ToString("yyyy-MM-dd hh:mm:ss.fffZ");
    }

    /// <summary>
    /// Returns the value of the configuration attribute of the assembly.
    /// </summary>
    public static string GetAssemblyConfiguration(this Assembly assembly)
    {
        var configurationAttribute = assembly
            .GetCustomAttribute<AssemblyConfigurationAttribute>()
            ?.Configuration;

        return string.IsNullOrWhiteSpace(configurationAttribute) ? "Debug" : configurationAttribute;
    }

    /// <summary>
    /// Returns the value of the product attribute of the assembly.
    /// </summary>
    public static string GetAssemblyProductName(this Assembly assembly)
    {
        var productAttribute = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

        return productAttribute ?? string.Empty;
    }
}
