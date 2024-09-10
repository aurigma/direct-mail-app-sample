using System.Net;
using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class TokenAdapter(
    HttpClient httpClient,
    IOptionsSnapshot<CustomersCanvasConfiguration> optionsSnapshot
) : ITokenAdapter
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly CustomersCanvasConfiguration _settings = optionsSnapshot.Value;

    public async Task<string> GetCustomersCanvasTokenAsync()
    {
        if (
            string.IsNullOrWhiteSpace(_settings.ClientId)
            || string.IsNullOrWhiteSpace(_settings.ClientSecret)
        )
        {
            throw new Exception("Customer's Canvas credentials is not provided.");
        }

        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = _settings.ApiGatewayUrl.TrimEnd('/') + "/connect/token", // IdentityProvider token url (Customer's Canvas token url)
                ClientId = _settings.ClientId,
                ClientSecret = _settings.ClientSecret,
            }
        );

        if (tokenResponse.IsError)
        {
            throw new Exception(
                "Could not retrieve token. " + HttpStatusCode.InternalServerError.ToString()
            );
        }

        return tokenResponse.AccessToken;
    }
}
