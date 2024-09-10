using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using RestSharp;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas.DesignEditor;

public class DesignEditorAdapter : IDesignEditorAdapter
{
    private IRestClient _client;

    public async Task<Token> CreateTokenAsync(
        string userId,
        string apiUrl,
        string apiKey,
        bool updateOnCall = true,
        int seconds = 7200
    )
    {
        _client = new RestClient(apiUrl);

        var request = new RestRequest($"/api/Auth/Users/{userId}/Tokens", Method.Post);

        request.AddHeader("X-CustomersCanvasAPIKey", apiKey);
        request.AddParameter("updateOnCall", updateOnCall, ParameterType.QueryString);
        request.AddParameter("seconds", seconds, ParameterType.QueryString);

        return await MakeRequest<Token>(request);
    }

    private async Task<T> MakeRequest<T>(RestRequest request)
    {
        var response = await _client.ExecuteAsync<T>(request);
        CheckErrors(response);
        return response.Data;
    }

    private void CheckErrors(RestResponse response)
    {
        if (response.ResponseStatus != ResponseStatus.Completed)
        {
            throw new DesignEditorAdapterException((int)response.StatusCode, response.Content);
        }

        if (
            response.StatusCode != System.Net.HttpStatusCode.OK
            && response.StatusCode != System.Net.HttpStatusCode.NoContent
        )
        {
            throw new DesignEditorAdapterException((int)response.StatusCode, response.Content);
        }
    }
}
