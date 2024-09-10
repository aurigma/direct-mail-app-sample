using System.Reflection;
using Aurigma.DirectMail.Sample.WebApi.Extensions;

namespace Aurigma.DirectMail.Sample.WebApi.Models.BuildInfo;

/// <summary>
/// Model with information about the application.
/// </summary>
public class BuildInfoModel(Assembly assembly)
{
    /// <summary>
    /// Version number.
    /// </summary>
    public string Version { get; } = assembly.GetVersion();

    /// <summary>
    /// Build date (UTC).
    /// </summary>
    public string BuildDate { get; } = assembly.GetAssemblyDate();

    /// <summary>
    /// Build configuration (Debug or Release).
    /// </summary>
    public string Configuration { get; } = assembly.GetAssemblyConfiguration();

    /// <summary>
    /// Application name.
    /// </summary>
    public string AppName { get; } = assembly.GetAssemblyProductName();
}
