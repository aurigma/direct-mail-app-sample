using Aurigma.StorefrontApi;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas storefront users.
/// </summary>
public interface IStorefrontUserAdapter
{
    /// <summary>
    /// Returns the model of an existing Storefront user, or creates a new one
    /// </summary>
    /// <param name="userId">Storefront user id.</param>
    /// <param name="token">Customer's Canvas</param>
    /// <returns>Storefront user DTO.</returns>
    Task<StorefrontUserDto> EnsureCreateStorefrontUserAsync(string userId, string token);
}
