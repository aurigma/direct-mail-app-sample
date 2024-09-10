using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.StorefrontApi;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class StorefrontUserAdapter(
    IStorefrontUsersApiClient storefrontUsersApiClient,
    IOptionsSnapshot<CustomersCanvasConfiguration> optionsSnapshot
) : IStorefrontUserAdapter
{
    private readonly IStorefrontUsersApiClient _storefrontUsersApiClient = storefrontUsersApiClient;
    private readonly CustomersCanvasConfiguration _configuration = optionsSnapshot.Value;

    public async Task<StorefrontUserDto> EnsureCreateStorefrontUserAsync(
        string userId,
        string token
    )
    {
        try
        {
            _storefrontUsersApiClient.AuthorizationToken = token;
            var storefrontUser = await _storefrontUsersApiClient.GetAsync(
                userId,
                _configuration.StorefrontId
            );

            return storefrontUser;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            var storefrontUser = await CreateStorefrontUserAsync(userId);
            return storefrontUser;
        }
    }

    private async Task<StorefrontUserDto> CreateStorefrontUserAsync(string userId)
    {
        var createUserModel = new CreateStorefrontUserDto
        {
            IsAnonymous = true,
            StorefrontUserId = userId,
        };

        var storefrontUser = await _storefrontUsersApiClient.CreateAsync(
            _configuration.StorefrontId,
            body: createUserModel
        );
        return storefrontUser;
    }
}
