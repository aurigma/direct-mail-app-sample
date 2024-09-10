using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Models.Image;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas private images.
/// </summary>
public interface IPrivateImageAdapter
{
    /// <summary>
    /// Returns the private image's folder, or creates a new one
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The private image folder DTO.</returns>
    Task<FolderDto> EnsureCreateFolderAsync(
        PrivateImageFolderCreationAdapterModel model,
        string token
    );

    /// <summary>
    /// Returns a collection of private images.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns> The collection of private images</returns>
    Task<IEnumerable<ImageDto>> GetPrivateImagesAsync(
        PrivateImagesRequestAdapterModel model,
        string token
    );
}
