using Aurigma.DirectMail.Sample.DomainEntities.Entities.Image;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with images.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Get all images.
    /// </summary>
    /// <returns>All images.</returns>
    Task<IEnumerable<Image>> GetImagesAsync();

    /// <summary>
    /// Get image by name.
    /// </summary>
    /// <param name="imageName">Image name.</param>
    /// <returns>The image.</returns>
    Task<Image> GetImageAsync(string imageName);
}
