using Aurigma.DirectMail.Sample.App.Exceptions.RecipientList;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.App.Models.Image;
using Aurigma.DirectMail.Sample.App.Models.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Image;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class RecipientListService(
    IRecipientListRepository repository,
    IImageService imageService,
    IPrivateImageAdapter privateImageAdapter,
    IStorefrontUserAdapter storefrontUserAdapter,
    ITokenAdapter tokenAdapter,
    IPrivateImageProcessorAdapter privateImageProcessorAdapter
) : IRecipientListService
{
    private readonly IRecipientListRepository _repository = repository;
    private readonly IImageService _imageService = imageService;
    private readonly IPrivateImageAdapter _privateImageAdapter = privateImageAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly IStorefrontUserAdapter _storefrontUserAdapter = storefrontUserAdapter;
    private readonly IPrivateImageProcessorAdapter _privateImageProcessorAdapter =
        privateImageProcessorAdapter;

    public async Task<List<RecipientList>> GetRecipientListsAsync(RecipientListFilter filter = null)
    {
        if (filter is null)
            return await _repository.GetAllAsReadOnlyAsync();

        return await _repository.GetRecipientListsByFilterAsync(filter);
    }

    public async Task<RecipientList> GetRecipientListAsync(Guid id)
    {
        return await _repository.GetRecipientListAsReadOnlyAsync(id)
            ?? throw new RecipientListNotFoundException(
                id,
                $"The recipient list with identifier {id} was not found"
            );
    }

    public async Task<List<RecipientList>> SubmitRecipientListsAsync(
        RecipientListSubmitAppModel model
    )
    {
        var campaignLists = await GetRecipientListsAsync(
            new RecipientListFilter { CampaignId = model.CampaignId }
        );
        if (!campaignLists.Any())
            return new List<RecipientList>();

        var submittingLists = campaignLists.Where(l => model.RecipientListIds.Contains(l.Id));

        var recipientImages = submittingLists.SelectMany(l =>
            l.Recipients.SelectMany(r => r.Images)
        );
        await ImportImagesToAssetStorageAsync(
            model.CampaignId,
            recipientImages.DistinctBy(i => i.Id).ToList()
        );

        // Other recipients list data submitting actions...

        return submittingLists.ToList();
    }

    private async Task ImportImagesToAssetStorageAsync(
        Guid campaignId,
        List<RecipientImage> recipientImages
    )
    {
        var images = new List<Image>();
        foreach (var recipientImage in recipientImages)
        {
            images.Add(await _imageService.GetImageAsync(recipientImage.Name));
        }

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            campaignId.ToString(),
            token
        );

        // Creation folder in Asset Storage.
        var createFolderAdapterModel = new PrivateImageFolderCreationAdapterModel
        {
            FolderName = campaignId.ToString(),
            Path = "/",
            UserId = storefrontUser.UserId,
        };

        var folder = await _privateImageAdapter.EnsureCreateFolderAsync(
            createFolderAdapterModel,
            token
        );
        var requestImagesModel = new PrivateImagesRequestAdapterModel
        {
            Path = Path.Combine(folder.Path, folder.Name),
            UserId = storefrontUser.UserId,
        };
        var earlierImportedImages = await _privateImageAdapter.GetPrivateImagesAsync(
            requestImagesModel,
            token
        );

        // Importing images to Asset Storage.
        foreach (var image in images)
        {
            if (earlierImportedImages.Any(ii => ii.Name == image.Name))
                continue;

            var contentStream = new MemoryStream(image.Content);
            var createPrivateImageAdapterModel = new PrivateImageImportationAdapterModel
            {
                ImageContent = contentStream,
                Name = image.Name,
                Path = Path.Combine(folder.Path, folder.Name),
                UserId = storefrontUser.UserId,
            };

            var importedPrivateImage = await _privateImageProcessorAdapter.CreatePrivateImageAsync(
                createPrivateImageAdapterModel,
                token
            );

            await contentStream.DisposeAsync();
        }
    }
}
