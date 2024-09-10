using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Image;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class PrivateImageAdapter(IPrivateImagesApiClient privateImagesApiClient)
    : IPrivateImageAdapter
{
    private readonly IPrivateImagesApiClient _privateImagesApiClient = privateImagesApiClient;

    public async Task<FolderDto> EnsureCreateFolderAsync(
        PrivateImageFolderCreationAdapterModel model,
        string token
    )
    {
        try
        {
            _privateImagesApiClient.AuthorizationToken = token;

            var folder = await _privateImagesApiClient.GetFolderAsync(
                model.FolderName,
                ownerId: model.UserId
            );
            var folderInfo = await _privateImagesApiClient.GetFolderInfoAsync(
                folder.Id,
                ownerId: model.UserId
            );
            return folderInfo;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            var createModel = new CreateFolderDto() { Name = model.FolderName, Path = model.Path };
            var folder = await _privateImagesApiClient.CreateFolderAsync(
                ownerId: model.UserId,
                body: createModel
            );
            return folder;
        }
    }

    public async Task<IEnumerable<ImageDto>> GetPrivateImagesAsync(
        PrivateImagesRequestAdapterModel model,
        string token
    )
    {
        _privateImagesApiClient.AuthorizationToken = token;

        var images = await _privateImagesApiClient.GetAllAsync(
            path: model.Path,
            includeSubfolders: false,
            ownerId: model.UserId
        );
        return images.Items;
    }
}
