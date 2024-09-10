using System;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Attributes;

/// <summary>
/// Produces model to response headers.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ProducesResponseHeaderAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProducesResponseHeaderAttribute"/> class.
    /// </summary>
    /// <param name="name">Response header name.</param>
    /// <param name="statusCode">HTTP status code for which the response header is being produced.</param>
    public ProducesResponseHeaderAttribute(string name, int statusCode)
    {
        Name = name;
        StatusCode = statusCode;
        Type = ResponseHeaderValueType.String;
    }

    /// <summary>
    /// Response header name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// HTTP status code for which the response header is being produced.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Response header value type.
    /// </summary>
    public ResponseHeaderValueType Type { get; set; }

    /// <summary>
    /// Response header description.
    /// </summary>
    public string Description { get; set; }
}
