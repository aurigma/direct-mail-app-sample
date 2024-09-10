using Aurigma.StorefrontApi;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations to requests an information about Customer's Canvas tenants.
/// </summary>
public interface ITenantInfoAdapter
{
    /// <summary>
    /// Returns an information about the tenant applications.
    /// </summary>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Information about the tenant applications.</returns>
    Task<TenantApplicationsInfoDto> GetTenantApplicationInfoAsync(string token);
}
