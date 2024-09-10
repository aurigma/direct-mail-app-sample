using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.StorefrontApi;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class TenantInfoAdapter(ITenantInfoApiClient tenantInfoApiClient) : ITenantInfoAdapter
{
    private readonly ITenantInfoApiClient _tenantInfoApiClient = tenantInfoApiClient;

    public async Task<TenantApplicationsInfoDto> GetTenantApplicationInfoAsync(string token)
    {
        _tenantInfoApiClient.AuthorizationToken = token;
        var tenantApplicationInfo = await _tenantInfoApiClient.GetApplicationsInfoAsync();

        return tenantApplicationInfo;
    }
}
