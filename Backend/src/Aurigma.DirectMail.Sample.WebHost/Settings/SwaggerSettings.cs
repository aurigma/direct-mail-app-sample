using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.WebHost.Settings;

/// <summary>
/// Swagger settings.
/// </summary>
public class SwaggerSettings
{
    /// <summary>
    /// URI-friendly name that uniquely identifies the document.
    /// </summary>
    public string DocumentName { get; init; }

    /// <summary>
    /// The version of the OpenAPI document.
    /// </summary>
    public string DocumentVersion { get; init; }

    /// <summary>
    /// Title of the application.
    /// </summary>
    public string AppTitle { get; init; }

    /// <summary>
    /// Paths to XML files.
    /// </summary>
    public List<string> XmlFilesPaths { get; init; }
}
