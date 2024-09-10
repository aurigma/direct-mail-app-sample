using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Resource;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class ResourceAdapter(IResourcesApiClient resourcesApiClient) : IResourceAdapter
{
    private readonly IResourcesApiClient _resourcesApiClient = resourcesApiClient;

    public async Task<IEnumerable<ResourceDto>> GetResourcesAsync(
        ResourceRequestAdapterModel model,
        string token
    )
    {
        _resourcesApiClient.AuthorizationToken = token;

        var resourcesPage = await _resourcesApiClient.GetAllAsync(
            @namespace: model.Namespace,
            sourceId: model.SourceId,
            search: model.Name
        );

        return resourcesPage.Items;
    }
}
