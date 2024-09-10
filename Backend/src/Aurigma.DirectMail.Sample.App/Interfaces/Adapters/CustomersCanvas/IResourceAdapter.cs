using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Models.Resource;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas Asset Storage resource service.
/// </summary>
public interface IResourceAdapter
{
    /// <summary>
    /// Returns a collection of resources by filter.
    /// </summary>
    /// <param name="model">Filter model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The collection of resources</returns>
    Task<IEnumerable<ResourceDto>> GetResourcesAsync(
        ResourceRequestAdapterModel model,
        string token
    );
}
