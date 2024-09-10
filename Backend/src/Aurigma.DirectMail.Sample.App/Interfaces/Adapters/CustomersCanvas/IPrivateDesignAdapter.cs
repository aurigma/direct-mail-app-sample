using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas private designs.
/// </summary>
public interface IPrivateDesignAdapter
{
    /// <summary>
    /// Returns a private design by ID from Asset Storage API.
    /// </summary>
    /// <param name="designId">Private design id.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Private design DTO.</returns>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">Private design was not found from Asset Storage.</exception>
    /// <remarks>Private design - personalized design, belongs to a specific Campaign.</remarks>
    Task<DesignDto> GetPrivateDesignByIdAsync(string designId, string userId, string token);

    /// <summary>
    /// Copies a private design.
    /// </summary>
    /// <param name="sourceDesignId">Source private design id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <returns></returns>
    Task<DesignDto> CopyPrivateDesignAsync(string sourceDesignId, string token, string userId);
}
