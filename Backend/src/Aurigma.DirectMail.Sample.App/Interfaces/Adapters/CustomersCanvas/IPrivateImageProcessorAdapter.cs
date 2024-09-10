using Aurigma.AssetProcessor;
using Aurigma.DirectMail.Sample.App.Models.Image;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas private images in Asset Processor.
/// </summary>
public interface IPrivateImageProcessorAdapter
{
    /// <summary>
    /// Imported image to Asset Storage.
    /// </summary>
    /// <param name="model">Importation model.</param>
    /// <param name="token">The Customer's Canvas authorization model.</param>
    /// <returns></returns>
    Task<ImageDto> CreatePrivateImageAsync(PrivateImageImportationAdapterModel model, string token);
}
