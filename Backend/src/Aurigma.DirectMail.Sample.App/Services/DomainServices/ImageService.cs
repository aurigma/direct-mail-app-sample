using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Image;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class ImageService(IFileService fileService) : IImageService
{
    private readonly IFileService _fileService = fileService;

    private const string PrivateImagesFolder = "App_Data/PrivateImages";

    public async Task<IEnumerable<Image>> GetImagesAsync()
    {
        var files = await _fileService.GetFilesAsync(PrivateImagesFolder);
        var images = files.Select(f => new Image
        {
            Name = f.Name,
            Path = f.Path,
            Content = f.Content,
        });
        return images;
    }

    public async Task<Image> GetImageAsync(string imageName)
    {
        var file = await _fileService.GetFileAsync(Path.Combine(PrivateImagesFolder, imageName));
        var image = new Image
        {
            Name = file.Name,
            Path = file.Path,
            Content = file.Content,
        };

        return image;
    }
}
