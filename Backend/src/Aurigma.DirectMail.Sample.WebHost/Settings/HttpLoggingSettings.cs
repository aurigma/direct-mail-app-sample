using Microsoft.AspNetCore.HttpLogging;

namespace Aurigma.DirectMail.Sample.WebHost.Settings;

/// <summary>
/// Settings of HTTP logging.
/// </summary>
public class HttpLoggingSettings
{
    /// <summary>
    /// Flag for logging the HTTP request header, <see cref="HttpLoggingFields.RequestHeaders"/>.
    /// </summary>
    public bool? RequestHeaders { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request protocol, <see cref="HttpLoggingFields.RequestProtocol"/>.
    /// </summary>
    public bool? RequestProtocol { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request scheme, <see cref="HttpLoggingFields.RequestScheme"/>.
    /// </summary>
    public bool? RequestScheme { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request method, <see cref="HttpLoggingFields.RequestMethod"/>.
    /// </summary>
    public bool? RequestMethod { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request path, <see cref="HttpLoggingFields.RequestPath"/>.
    /// </summary>
    public bool? RequestPath { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request query, <see cref="HttpLoggingFields.RequestQuery"/>.
    /// </summary>
    public bool? RequestQuery { get; init; }

    /// <summary>
    /// Flag for logging the HTTP request body, <see cref="HttpLoggingFields.RequestBody"/>.
    /// </summary>
    public bool? RequestBody { get; init; }

    /// <summary>
    /// Flag for logging the HTTP response headers, <see cref="HttpLoggingFields.ResponseHeaders"/>.
    /// </summary>
    public bool? ResponseHeaders { get; init; }

    /// <summary>
    /// Flag for logging the HTTP response status code, <see cref="HttpLoggingFields.ResponseStatusCode"/>.
    /// </summary>
    public bool? ResponseStatusCode { get; init; }

    /// <summary>
    /// Flag for logging the HTTP Response body, <see cref="HttpLoggingFields.ResponseBody"/>.
    /// </summary>
    public bool? ResponseBody { get; init; }
}
