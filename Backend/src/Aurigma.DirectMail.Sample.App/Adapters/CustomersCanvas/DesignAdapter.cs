using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class DesignAdapter(IDesignsApiClient designsApiClient) : IDesignAdapter
{
    private readonly IDesignsApiClient _designsApiClient = designsApiClient;

    public async Task<DesignDto> GetDesignAsync(string designId, string token)
    {
        try
        {
            _designsApiClient.AuthorizationToken = token;

            var design = await _designsApiClient.GetAsync(designId);
            return design;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasDesignNotFoundException(
                designId,
                $"The design with identifier {designId} was not found",
                ex
            );
        }
    }
}
