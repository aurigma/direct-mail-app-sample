using System.IO.Compression;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.Preview;
using Aurigma.DirectMail.Sample.App.Extensions;
using Aurigma.DirectMail.Sample.App.Helpers;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Image;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.App.Models.VDP;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Preview;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using Aurigma.StorefrontApi.Products;
using AutoMapper;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class PreviewService(
    ILineItemService lineItemService,
    ICampaignService campaignService,
    IDesignAtomsServiceAdapter designAtomsServiceAdapter,
    ITokenAdapter tokenAdapter,
    IVdpService vdpService,
    IPrivateDesignAdapter privateDesignAdapter,
    IStorefrontUserAdapter storefrontUserAdapter,
    IProductReferenceAdapter productReferenceAdapter,
    IProductAdapter productAdapter,
    IPrivateImageAdapter privateImageAdapter
) : IPreviewService
{
    private readonly ILineItemService _lineItemService = lineItemService;
    private readonly ICampaignService _campaignService = campaignService;
    private readonly IDesignAtomsServiceAdapter _designAtomsServiceAdapter =
        designAtomsServiceAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly IVdpService _vdpService = vdpService;
    private readonly IPrivateDesignAdapter _privateDesignAdapter = privateDesignAdapter;
    private readonly IStorefrontUserAdapter _storefrontUserAdapter = storefrontUserAdapter;
    private readonly IPrivateImageAdapter _privateImageAdapter = privateImageAdapter;
    private readonly IProductReferenceAdapter _productReferenceAdapter = productReferenceAdapter;
    private readonly IProductAdapter _productAdapter = productAdapter;

    private readonly int DelayBetweenRequestsInMs = 500;

    private const string ImageVariableName = "Image";
    private const string ImagePlaceholderVariableName = "ImagePlaceholder";

    public async Task<DesignInfo> GetDesignInfoAsync(DesignInfoRequestAppModel model)
    {
        ValidateIds(model);
        var lineItem = await _lineItemService.GetLineItemAsync(model.LineItemId);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new PreviewLineItemException(
                lineItem.Id,
                $"The line item with identifier {lineItem.Id} has no defined design"
            );

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            lineItem.CampaignId.ToString(),
            token
        );
        var privateDesign = await _privateDesignAdapter.GetPrivateDesignByIdAsync(
            lineItem.DesignId,
            storefrontUser.UserId,
            token
        );

        var designInfo = new DesignInfo
        {
            Id = privateDesign.Id,
            SurfaceCount = privateDesign.Metadata.Surfaces.Count,
        };

        return designInfo;
    }

    public async Task<Stream> RenderDesignPreviewAsync(PreviewRequestAppModel model)
    {
        var lineItem = await _lineItemService.GetLineItemAsync(model.LineItemId);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new PreviewLineItemException(
                lineItem.Id,
                $"The line item with identifier {lineItem.Id} has no defined design"
            );

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            lineItem.CampaignId.ToString(),
            token
        );

        Stream designPreview;
        if (model.RecipientId.HasValue)
            designPreview = await RenderPreviewWithVdpData(
                lineItem,
                model.Config,
                model.RecipientId.Value,
                token,
                storefrontUser.UserId
            );
        else
            designPreview = await RenderPreviewWithoutVdpData(
                lineItem,
                model.Config,
                token,
                storefrontUser.UserId
            );

        return designPreview;
    }

    public async Task<ProofsZip> GetProofsZipAsync(
        ProofZipRequestAppModel model,
        int recipientsCount = 3
    )
    {
        // Prepare data to render a previews.
        ValidateIds(model);
        var lineItem = await _lineItemService.GetLineItemAsync(model.LineItemId);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new PreviewLineItemException(
                lineItem.Id,
                $"The line item with identifier {lineItem.Id} has no defined design"
            );

        var campaign = await _campaignService.GetCampaignAsync(lineItem.CampaignId);
        var campaignRecipients = await _campaignService.GetCampaignRecipientsAsync(campaign);

        if (campaignRecipients.Count() < recipientsCount)
            recipientsCount = campaignRecipients.Count();

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            lineItem.CampaignId.ToString(),
            token
        );

        var randomRecipients = campaignRecipients.PickRandom(recipientsCount);

        // Request design and private images.
        var privateDesign = await _privateDesignAdapter.GetPrivateDesignByIdAsync(
            lineItem.DesignId,
            storefrontUser.UserId,
            token
        );

        // Render previews.
        var files = new Dictionary<string, Stream>();
        foreach (var (recipient, index) in randomRecipients.WithIndex())
        {
            var vdpImages = await GetVdpImages(
                lineItem,
                recipient.Images,
                token,
                storefrontUser.UserId
            );
            var recipientVariables = _vdpService.GetRecipientVariableData(
                recipient,
                vdpImages.ToList()
            );
            for (var i = 0; i < privateDesign.Metadata.Surfaces.Count; i++)
            {
                var requestModel = BuildRenderProofAdapterModel(
                    config: model.Config,
                    variables: recipientVariables.ToList(),
                    surfaceIndex: i,
                    designId: privateDesign.Id,
                    userId: storefrontUser.UserId
                );

                var proofFile = await _designAtomsServiceAdapter.RenderDesignProofAsync(
                    requestModel,
                    token
                );
                files.Add("recipient_" + index + "_" + i, proofFile.Stream);
                await Task.Delay(DelayBetweenRequestsInMs);
            }
        }

        // Create zip archive.
        var zipName = $"previews_{DateTime.UtcNow.ToString("yyyy_MM_dd_hh_mm_ss")}.zip";
        var zipStream = await PrepareZipArchiveForProofs(files);

        foreach (var file in files)
        {
            await file.Value.DisposeAsync();
        }

        return new ProofsZip() { FileDownloadName = zipName, ZipData = zipStream.ToArray() };
    }

    public async Task<Stream> RenderProofForRecipientAsync(ProofRequestAppModel model)
    {
        // Prepare data to render the preview.
        ValidateIds(model);
        var lineItem = await _lineItemService.GetLineItemAsync(model.LineItemId);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new PreviewLineItemException(
                lineItem.Id,
                $"The line item with identifier {lineItem.Id} has no defined design"
            );

        // Request design and private images.
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            lineItem.CampaignId.ToString(),
            token
        );

        // Build variables.
        var recipientVariables = await BuildVdpDataForRecipient(
            lineItem,
            model.RecipientId,
            token,
            storefrontUser.UserId
        );

        // Render preview.
        var renderProofAdapterModel = BuildRenderProofAdapterModel(
            config: model.Config,
            variables: recipientVariables.ToList(),
            designId: lineItem.DesignId,
            userId: storefrontUser.UserId
        );

        var proofFile = await _designAtomsServiceAdapter.RenderDesignProofAsync(
            renderProofAdapterModel,
            token
        );

        return proofFile.Stream;
    }

    private async Task<Stream> RenderPreviewWithVdpData(
        LineItem lineItem,
        PreviewRequestConfigAppModel config,
        Guid recipientId,
        string token,
        string userId
    )
    {
        var product = await _productReferenceAdapter.GetProductByReferenceAsync(
            token,
            lineItem.ProductId.ToString()
        );
        var variantDesigns = await _productAdapter.GetProductVariantDesignsAsync(
            productId: product.ProductId,
            options: null,
            templateTitle: null,
            token: token,
            productVariantId: lineItem.ProductVariantId
        );

        var variantMockups = await _productAdapter.GetProductVarianMockupsAsync(
            productId: product.ProductId,
            options: null,
            templateTitle: null,
            token: token,
            productVariantId: lineItem.ProductVariantId
        );
        var previewVariantMockups = variantMockups.Where(m =>
            m.MockupType is ProductVariantMockupType.Preview
        );

        var currentVariantDesign = variantDesigns.FirstOrDefault(v =>
            v.DesignId == lineItem.TemplateId
        );

        if (currentVariantDesign is null)
            throw new CustomersCanvasDesignNotConnectedException(
                lineItem.TemplateId,
                lineItem.ProductId.ToString(),
                $"Product with reference {lineItem.ProductId} does not have designs connected to this product variant: {lineItem.ProductVariantId}"
            );

        // Build variables.
        var recipientVariables = await BuildVdpDataForRecipient(
            lineItem,
            recipientId,
            token,
            userId
        );

        // Render preview.
        var requestModel = BuildPreviewRequestAdapterModel(
            designId: lineItem.DesignId,
            userId: userId,
            mockupId: previewVariantMockups
                .FirstOrDefault(pm => pm.SurfaceIndex == config.SurfaceIndex)
                ?.MockupId,
            height: config.Height,
            width: config.Width,
            surfaceIndex: config.SurfaceIndex,
            variables: recipientVariables
        );

        var designPreview = await _designAtomsServiceAdapter.RenderDesignPreviewAsync(
            requestModel,
            token
        );
        return designPreview.Stream;
    }

    private async Task<Stream> RenderPreviewWithoutVdpData(
        LineItem lineItem,
        PreviewRequestConfigAppModel config,
        string token,
        string userId
    )
    {
        var product = await _productReferenceAdapter.GetProductByReferenceAsync(
            token,
            lineItem.ProductId.ToString()
        );
        var variantDesigns = await _productAdapter.GetProductVariantDesignsAsync(
            productId: product.ProductId,
            options: null,
            templateTitle: null,
            token: token,
            productVariantId: lineItem.ProductVariantId
        );

        var variantMockups = await _productAdapter.GetProductVarianMockupsAsync(
            productId: product.ProductId,
            options: null,
            templateTitle: null,
            token: token,
            productVariantId: lineItem.ProductVariantId
        );
        var previewVariantMockups = variantMockups.Where(m =>
            m.MockupType is ProductVariantMockupType.Preview
        );

        var currentVariantDesign = variantDesigns.FirstOrDefault(v =>
            v.DesignId == lineItem.TemplateId
        );

        if (currentVariantDesign is null)
            throw new CustomersCanvasDesignNotConnectedException(
                lineItem.TemplateId,
                lineItem.ProductId.ToString(),
                $"Product with reference {lineItem.ProductId} does not have designs connected to this product variant: {lineItem.ProductVariantId}"
            );

        var requestModel = BuildPreviewRequestAdapterModel(
            designId: lineItem.DesignId,
            userId: userId,
            mockupId: previewVariantMockups
                .FirstOrDefault(pm => pm.SurfaceIndex == config.SurfaceIndex)
                ?.MockupId,
            height: config.Height,
            width: config.Width,
            surfaceIndex: config.SurfaceIndex
        );

        var designPreview = await _designAtomsServiceAdapter.RenderDesignPreviewAsync(
            requestModel,
            token
        );
        return designPreview.Stream;
    }

    private async Task<IEnumerable<Variable>> BuildVdpDataForRecipient(
        LineItem lineItem,
        Guid recipientId,
        string token,
        string userId
    )
    {
        var campaign = await _campaignService.GetCampaignAsync(lineItem.CampaignId);
        var campaignRecipients = await _campaignService.GetCampaignRecipientsAsync(campaign);

        var recipient = campaignRecipients.FirstOrDefault(r => r.Id == recipientId);
        if (recipient == null)
            throw new PreviewRecipientException(
                $"The recipient with identifier {recipientId} was not found in recipient lists."
            );

        var recipientImages = recipient.Images;

        var vdpImages = await GetVdpImages(lineItem, recipientImages, token, userId);

        // Build variables.
        var recipientVariables = _vdpService.GetRecipientVariableData(
            recipient,
            vdpImages.ToList()
        );

        return recipientVariables;
    }

    private async Task<IEnumerable<VdpBuildDataSetImageAppModel>> GetVdpImages(
        LineItem lineItem,
        IEnumerable<RecipientImage> images,
        string token,
        string userId
    )
    {
        var designVariables = await _designAtomsServiceAdapter.GetVariablesAsync(
            lineItem.DesignId,
            userId,
            token
        );
        var imageDesignVariables = designVariables.Items.Where(dv =>
            dv.Type is ImageVariableName || dv.Type is ImagePlaceholderVariableName
        );
        if (!imageDesignVariables.Any())
            return new List<VdpBuildDataSetImageAppModel>();

        var privateImages = await GetImportedPrivateImagesAsync(
            lineItem.CampaignId.ToString(),
            userId,
            token
        );
        var importedRecipientImages = images
            .Where(i =>
                privateImages.Any(pi =>
                    pi.Name.Contains(i.Name, StringComparison.OrdinalIgnoreCase)
                )
            )
            .ToList();

        var vdpImages = new List<VdpBuildDataSetImageAppModel>();

        // We assume that there are only one variable

        var imageVariable = imageDesignVariables.FirstOrDefault(i =>
            string.Equals(
                i.Name,
                RecipientVariableNamesHelper.Image,
                StringComparison.OrdinalIgnoreCase
            )
        );
        if (imageVariable != null)
        {
            var recipientCampaignImage = importedRecipientImages.FirstOrDefault(ri =>
                ri.Name.Contains("image")
            );
            var vdpImage = new VdpBuildDataSetImageAppModel()
            {
                Name = imageVariable.Name,
                Value = $"user:{userId}/{recipientCampaignImage?.Name}",
                Type = ToPrivateVdpImageType(imageVariable.Type),
            };

            vdpImages.Add(vdpImage);
        }

        return vdpImages;
    }

    private ProofRequestAdapterModel BuildRenderProofAdapterModel(
        ProofRequestConfigAppModel config,
        List<Variable> variables,
        string designId,
        string userId
    )
    {
        return new ProofRequestAdapterModel
        {
            DesignId = designId,
            UserId = userId,
            Config = new DesignAtomsApi.ProductProofRenderingConfig
            {
                Format = DesignAtomsApi.ProductProofFormat.Pdf,
                Height = config.Height,
                Width = config.Width,
                SurfaceIndex = config.SurfaceIndex,
                WatermarkEnabled = false,
                SafetyLinesEnabled = false,
                WatermarkRepeat = false,
            },
            Variables = variables.Select(v => new DesignAtomsApi.VariableInfo
            {
                Name = v.Name,
                Value = v.Value,
                Type = ToDesignAtomsVariableInfoType(v.Type),
            }),
        };
    }

    private ProofRequestAdapterModel BuildRenderProofAdapterModel(
        ProofZipRequestConfigAppModel config,
        List<Variable> variables,
        int surfaceIndex,
        string designId,
        string userId
    )
    {
        return new ProofRequestAdapterModel
        {
            DesignId = designId,
            UserId = userId,
            Config = new DesignAtomsApi.ProductProofRenderingConfig
            {
                Format = DesignAtomsApi.ProductProofFormat.Pdf,
                Height = config.Height,
                Width = config.Width,
                SurfaceIndex = surfaceIndex,
                WatermarkEnabled = false,
                SafetyLinesEnabled = false,
                WatermarkRepeat = false,
            },
            Variables = variables.Select(v => new DesignAtomsApi.VariableInfo
            {
                Name = v.Name,
                Value = v.Value,
                Type = ToDesignAtomsVariableInfoType(v.Type),
            }),
        };
    }

    private PreviewRequestAdapterModel BuildPreviewRequestAdapterModel(
        string designId,
        string userId,
        string mockupId,
        int height,
        int width,
        int surfaceIndex,
        IEnumerable<Variable> variables = null
    )
    {
        return new PreviewRequestAdapterModel
        {
            DesignId = designId,
            MockupId = mockupId,
            UserId = userId,
            Config = new DesignAtomsApi.ProductPreviewRenderingConfig()
            {
                FileFormat = DesignAtomsApi.ProductPreviewFormat.Jpeg,
                Height = height,
                Width = width,
                SurfaceIndex = surfaceIndex,
            },
            Variables = variables?.Select(v => new DesignAtomsApi.VariableInfo
            {
                Name = v.Name,
                Value = v.Value,
                Type = ToDesignAtomsVariableInfoType(v.Type),
            }),
        };
    }

    private async Task<MemoryStream> PrepareZipArchiveForProofs(Dictionary<string, Stream> files)
    {
        var result = new MemoryStream();
        using (var archive = new ZipArchive(result, ZipArchiveMode.Create, true))
        {
            var count = 0;
            foreach (var stream in files)
            {
                var entry = archive.CreateEntry(stream.Key + ".pdf");
                using (var entryStream = entry.Open())
                {
                    await stream.Value.CopyToAsync(entryStream);
                }

                count++;
            }
        }

        result.Position = 0;
        return result;
    }

    private async Task<List<AssetStorage.ImageDto>> GetImportedPrivateImagesAsync(
        string path,
        string userId,
        string token
    )
    {
        var requestPrivateImagesModel = new PrivateImagesRequestAdapterModel
        {
            Path = $"/{path}",
            UserId = userId,
        };

        var privateImages = await _privateImageAdapter.GetPrivateImagesAsync(
            requestPrivateImagesModel,
            token
        );

        return privateImages.ToList();
    }

    private void ValidateIds(ProofRequestAppModel model)
    {
        if (model.LineItemId == default)
            throw new PreviewIdException("The line item ID is not specified.", model.LineItemId);

        if (model.RecipientId == default)
            throw new PreviewIdException("The recipient ID is not specified.", model.RecipientId);
    }

    private void ValidateIds(ProofZipRequestAppModel model)
    {
        if (model.LineItemId == default)
            throw new PreviewIdException("The line item ID is not specified.", model.LineItemId);
    }

    private void ValidateIds(DesignInfoRequestAppModel model)
    {
        if (model.LineItemId == default)
            throw new PreviewIdException("The line item ID is not specified.", model.LineItemId);
    }

    private string ToDesignAtomsVariableInfoType(VariableType type) =>
        type switch
        {
            VariableType.PublicImage => "Image",
            VariableType.PrivateImage => "Image",
            VariableType.ExternalImage => "Image",
            VariableType.PublicPlaceholder => "ImagePlaceholder",
            VariableType.PrivatePlaceholder => "ImagePlaceholder",
            VariableType.ExternalPlaceholder => "ImagePlaceholder",
            VariableType.InString => "InString",
            VariableType.Barcode => "Barcode",
            VariableType.BarcodePlaceholder => "BarcodePlaceholder",
            _ => throw new NotImplementedException(
                $"Unsupported variable type. variableType={type}"
            ),
        };

    private VdpImageType ToPrivateVdpImageType(string designAtomsVariableType) =>
        designAtomsVariableType switch
        {
            "Image" => VdpImageType.Private,
            "ImagePlaceholder" => VdpImageType.PrivatePlaceholder,
            _ => VdpImageType.Private,
        };
}

internal class PreviewServiceMapperProfile : Profile
{
    public PreviewServiceMapperProfile()
    {
        CreateMap<Variable, DesignAtomsApi.VariableInfo>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}
