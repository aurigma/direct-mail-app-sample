using System.Net.Http;

namespace Aurigma.DirectMail.Sample.WebHost.Providers;

/// <summary>
/// Provider of HTTP client.
/// </summary>
public class HttpClientProvider(IHttpClientFactory factory)
{
    private readonly IHttpClientFactory _factory = factory;

    /// <summary>
    /// Returns HTTP client by name.
    /// </summary>
    /// <param name="clientName">Client name.</param>
    /// <returns>Created <see cref="HttpClient"/>.</returns>
    public HttpClient GetHttpClient(string clientName)
    {
        return _factory.CreateClient(clientName);
    }
}
