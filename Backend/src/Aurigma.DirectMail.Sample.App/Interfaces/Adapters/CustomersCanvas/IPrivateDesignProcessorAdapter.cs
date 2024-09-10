using Aurigma.AssetProcessor;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

public interface IPrivateDesignProcessorAdapter
{
    /// <summary>
    /// Creates a private design from public design.
    /// </summary>
    /// <param name="publicDesignId">Public design id</param>
    /// <param name="userId">Storefront user id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The private design.</returns>
    /// <exception cref="CustomersCanvasDesignNotFoundException">Design was not found from Asset Storage.</exception>
    Task<DesignDto> GenerateDesignFromPublicDesignAsync(
        string publicDesignId,
        string userId,
        string token
    );

    /// <summary>
    /// Creates a private design from public resource.
    /// </summary>
    /// <param name="publicResourceId">Public resource id.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The private design.</returns>
    /// <exception cref="CustomersCanvasResourceNotFoundException">Resource was not found from Asset Storage.</exception>
    Task<DesignDto> GenerateDesignFromPublicResourceAsync(
        string publicResourceId,
        string userId,
        string token
    );
}
