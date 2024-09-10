using Aurigma.AssetProcessor;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Image;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class PrivateImageProcessorAdapter(
    IPrivateImageProcessorApiClient privateImageProcessorApiClient
) : IPrivateImageProcessorAdapter
{
    private readonly IPrivateImageProcessorApiClient _privateImageProcessorApiClient =
        privateImageProcessorApiClient;

    public async Task<ImageDto> CreatePrivateImageAsync(
        PrivateImageImportationAdapterModel model,
        string token
    )
    {
        _privateImageProcessorApiClient.AuthorizationToken = token;

        var fileParameter = new FileParameter(model.ImageContent);
        var image = await _privateImageProcessorApiClient.ImportImageAsync(
            ownerId: model.UserId,
            path: model.Path,
            name: model.Name,
            sourceFile: fileParameter
        );

        return image;
    }
}
