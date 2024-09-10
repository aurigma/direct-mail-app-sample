using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas designs.
/// </summary>
public interface IDesignAdapter
{
    /// <summary>
    /// Returns a design by ID from Asset Storage API.
    /// </summary>
    /// <param name="designId">Design id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Private design DTO.</returns>
    /// <exception cref="CustomersCanvasDesignNotFoundException">Design was not found from Asset Storage.</exception>
    /// <remarks>Design - design (template), publicly available for any Campaign.</remarks>
    Task<DesignDto> GetDesignAsync(string designId, string token);
}
