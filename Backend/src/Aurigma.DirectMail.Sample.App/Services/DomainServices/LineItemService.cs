using Aurigma.DesignAtoms.ExtensionMethods;
using Aurigma.DesignAtoms.Model;
using Aurigma.DesignAtomsApi;
using Aurigma.DirectMail.Sample.App.Exceptions.Campaign;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Helpers;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.App.Models.Image;
using Aurigma.DirectMail.Sample.App.Models.Project;
using Aurigma.DirectMail.Sample.App.Models.VDP;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using Aurigma.StorefrontApi;
using AutoMapper;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class LineItemService(
    ILineItemRepository repository,
    ICampaignService campaignService,
    IProductService productService,
    IJobService jobService,
    IVdpService vdpService,
    IProjectService projectService,
    ICompanyService companyService,
    IDesignAdapter designAdapter,
    IPrivateDesignAdapter privateDesignAdapter,
    IPrivateImageAdapter privateImageAdapter,
    IStorefrontUserAdapter storefrontUserAdapter,
    IProductAdapter productAdapter,
    IProductReferenceAdapter productReferenceAdapter,
    IDesignAtomsServiceAdapter designAtomsServiceAdapter,
    ITokenAdapter tokenAdapter,
    IMapper mapper
) : ILineItemService
{
    private readonly ILineItemRepository _repository = repository;
    private readonly ICampaignService _campaignService = campaignService;
    private readonly IProductService _productService = productService;
    private readonly IJobService _jobService = jobService;
    private readonly IVdpService _vdpService = vdpService;
    private readonly IProjectService _projectService = projectService;
    private readonly IDesignAdapter _designAdapter = designAdapter;
    private readonly ICompanyService _companyService = companyService;
    private readonly IPrivateDesignAdapter _privateDesignAdapter = privateDesignAdapter;
    private readonly IPrivateImageAdapter _privateImageAdapter = privateImageAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly IStorefrontUserAdapter _storefrontUserAdapter = storefrontUserAdapter;
    private readonly IProductReferenceAdapter _productReferenceAdapter = productReferenceAdapter;
    private readonly IProductAdapter _productAdapter = productAdapter;
    private readonly IDesignAtomsServiceAdapter _designAtomsServiceAdapter =
        designAtomsServiceAdapter;
    private readonly IMapper _mapper = mapper;

    private const string ImageVariableName = "Image";
    private const string ImagePlaceholderVariableName = "ImagePlaceholder";

    public async Task<LineItem> CreateLineItemAsync(LineItem lineItem)
    {
        ValidateIds(lineItem);
        await ValidateCampaign(lineItem.CampaignId);
        await ValidateProduct(lineItem.ProductId);

        return await _repository.CreateLineItemAsync(lineItem);
    }

    public async Task<LineItem> GetLineItemAsync(Guid id)
    {
        return await _repository.GetLineItemByIdAsReadOnlyAsync(id)
            ?? throw new LineItemNotFoundException(
                id,
                $"The line item with identifier '{id}' was not found"
            );
    }

    public async Task<List<LineItem>> GetLineItemsAsync(LineItemFilter filter)
    {
        return await _repository.GetLineItemsByFilterAsync(filter);
    }

    public async Task<LineItem> UpdateLineItemAsync(LineItem lineItem)
    {
        ValidateIds(lineItem);
        await ValidateCampaign(lineItem.CampaignId);
        await ValidateProduct(lineItem.ProductId);

        var token = string.Empty;

        if (
            !string.IsNullOrWhiteSpace(lineItem.TemplateId)
            || !string.IsNullOrWhiteSpace(lineItem.DesignId)
        )
            token = await _tokenAdapter.GetCustomersCanvasTokenAsync();

        if (!string.IsNullOrWhiteSpace(lineItem.TemplateId))
            await ValidateTemplate(lineItem.TemplateId, token);

        if (!string.IsNullOrWhiteSpace(lineItem.DesignId))
            await ValidateDesign(lineItem.DesignId, lineItem.CampaignId.ToString(), token);

        if (lineItem.ProductVariantId.HasValue)
            await ValidateProductVariant(
                lineItem.ProductVariantId.Value,
                lineItem.ProductId.ToString(),
                token
            );

        var dbLineItem =
            await _repository.GetLineItemByIdAsReadOnlyAsync(lineItem.Id)
            ?? throw new LineItemNotFoundException(
                lineItem.Id,
                $"The line item with identifier '{lineItem.Id}' was not found"
            );

        _mapper.Map(lineItem, dbLineItem);

        return await _repository.UpdateLineItemAsync(dbLineItem);
    }

    public async Task FinishLineItemPersonalizationAsync(Guid id)
    {
        // Prepare data for saving VDP data to design.
        var lineItem = await GetLineItemAsync(id);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new LineItemDesignException(
                lineItem.DesignId,
                $"The line item with identifier {lineItem.Id} has no defined design."
            );

        var campaign = await ValidateCampaign(lineItem.CampaignId);
        var recipients = await _campaignService.GetCampaignRecipientsAsync(campaign);
        var company = await _companyService.GetCompanyAsync(campaign.CompanyId);

        // Process creation
        var project = await ProcessFinishLineItemPersonalizationAsync(
            lineItem,
            campaign,
            company,
            recipients
        );
        var processingResults = await GetProcessingResultsAsync(project.Id);

        // Creation a job.
        var job = await _jobService.CreateJobAsync(
            new Job
            {
                Id = Guid.NewGuid(),
                CustomersCanvasProjectId = project.Id,
                Status = _mapper.Map<JobStatus>(processingResults.Status),
                LineItemId = lineItem.Id,
            }
        );
    }

    private async Task<ProjectDto> ProcessFinishLineItemPersonalizationAsync(
        LineItem lineItem,
        Campaign campaign,
        Company company,
        IEnumerable<Recipient> recipients
    )
    {
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            campaign.Id.ToString(),
            token
        );
        var copiedDesign = await _privateDesignAdapter.CopyPrivateDesignAsync(
            lineItem.DesignId,
            token,
            storefrontUser.UserId
        );

        // Get imported private images.
        var privateImages = await GetImportedPrivateImagesAsync(
            userId: storefrontUser.UserId,
            path: campaign.Id.ToString(),
            token: token
        );

        var vdpImages = new List<VdpBuildDataSetImageAppModel>();
        var designVariables = await _designAtomsServiceAdapter.GetVariablesAsync(
            copiedDesign.Id,
            storefrontUser.UserId,
            token
        );
        // You can add another images (public) to vdpImages...

        // Saving VDP data to design.
        var surfacesCount = copiedDesign.Metadata.Surfaces.Count;
        var dataSet = BuildVdpDataSet(
            designVariables,
            privateImages,
            recipients.ToList(),
            surfacesCount,
            storefrontUser.UserId
        );

        var saveVdpAdapterModel = new VdpSendDataAppModel
        {
            DataSet = dataSet,
            DesignId = copiedDesign.Id,
            UserId = campaign.Id.ToString(),
        };

        await _vdpService.SendVdpDataAsync(saveVdpAdapterModel);

        var productOptions = await GetProductVariantOptionsAsync(
            lineItem.ProductVariantId.Value,
            lineItem.ProductId.ToString(),
            token
        );

        // Creation a Customers Canvas project.
        var projectCreationModel = new ProjectCreationAppModel()
        {
            DesignId = copiedDesign.Id,
            Campaign = campaign,
            LineItem = lineItem,
            Company = company,
            Fields = productOptions
                .Select(o => new KeyValuePair<string, object>(
                    o.ProductOptionTitle,
                    o.SimpleOptionValue.Value
                ))
                .ToDictionary(),
        };

        var projectDto = await _projectService.CreateProjectAsync(projectCreationModel);

        return projectDto;
    }

    private IEnumerable<VdpBuildDataSetImageAppModel> GetVdpImages(
        List<DesignAtomsApi.VariableInfo> variables,
        List<AssetStorage.ImageDto> privateImages,
        IEnumerable<RecipientImage> images,
        string userId
    )
    {
        var imageDesignVariables = variables.Where(dv =>
            dv.Type is ImageVariableName || dv.Type is ImagePlaceholderVariableName
        );
        if (!imageDesignVariables.Any())
            return new List<VdpBuildDataSetImageAppModel>();

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
                Value = $"{userId}/{recipientCampaignImage?.Name}",
                Type = ToPrivateVdpImageType(imageVariable.Type),
            };

            vdpImages.Add(vdpImage);
        }

        return vdpImages;
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

    private async Task<
        IEnumerable<StorefrontApi.Products.ProductVariantOptionDto>
    > GetProductVariantOptionsAsync(int productVariantId, string productReference, string token)
    {
        var product = await _productReferenceAdapter.GetProductByReferenceAsync(
            token,
            productReference
        );
        var productVariant = await _productAdapter.GetProductVariantAsync(
            product.ProductId,
            productVariantId,
            token
        );
        var productOptions = productVariant.ProductVariantOptions;

        return productOptions;
    }

    private DataSet BuildVdpDataSet(
        VariablesModel variablesPage,
        List<AssetStorage.ImageDto> privateImages,
        List<Recipient> recipients,
        int surfacesCount,
        string userId
    )
    {
        var variables = new Dictionary<Guid, List<Variable>>();

        foreach (var recipient in recipients)
        {
            var vdpImages = GetVdpImages(
                variablesPage.Items.ToList(),
                privateImages,
                recipient.Images,
                userId
            );
            var recipientVariables = _vdpService.GetRecipientVariableData(
                recipient,
                vdpImages.ToList()
            );
            variables.Add(recipient.Id, recipientVariables.ToList());
        }

        var surfaceIndexes = Enumerable.Range(0, surfacesCount).ToArray();
        var dataSet = _vdpService.BuildVdpDataSet(surfaceIndexes, variables);

        return dataSet;
    }

    private async Task<ProjectProcessingResultsDto> GetProcessingResultsAsync(int projectId)
    {
        var processingResult = await _projectService.GetProcessingResultsAsync(
            new ProcessingResultsRequestAppModel { ProjectId = projectId }
        );

        return processingResult;
    }

    private async Task ValidateProduct(Guid productId)
    {
        try
        {
            var product = await _productService.GetProductAsync(productId);
        }
        catch (ProductNotFoundException)
        {
            throw new LineItemProductException($"The product with id {productId} was not found");
        }
    }

    private async Task<Campaign> ValidateCampaign(Guid campaignId)
    {
        try
        {
            var campaign = await _campaignService.GetCampaignAsync(campaignId);
            return campaign;
        }
        catch (CampaignNotFoundException)
        {
            throw new LineItemCampaignException(
                campaignId,
                $"The campaign with id {campaignId} was not found"
            );
        }
    }

    private void ValidateIds(LineItem lineItem)
    {
        if (lineItem.Id == default)
        {
            throw new LineItemIdException("The ID of the line item  is not specified", lineItem.Id);
        }

        if (lineItem.CampaignId == default)
        {
            throw new LineItemIdException(
                "The line item's company ID is not specified.",
                lineItem.CampaignId
            );
        }

        if (lineItem.ProductId == default)
        {
            throw new LineItemIdException(
                "The line item's product ID is not specified.",
                lineItem.ProductId
            );
        }
    }

    private async Task ValidateTemplate(string templateId, string token)
    {
        var template = await _designAdapter.GetDesignAsync(templateId, token);
    }

    private async Task ValidateDesign(string designId, string userId, string token)
    {
        var design = await _privateDesignAdapter.GetPrivateDesignByIdAsync(designId, userId, token);
    }

    private async Task ValidateProductVariant(int variantId, string productReference, string token)
    {
        var product = await _productReferenceAdapter.GetProductByReferenceAsync(
            token,
            productReference
        );
        var productVariant = await _productAdapter.GetProductVariantAsync(
            product.ProductId,
            variantId,
            token
        );
    }

    private VdpImageType ToPrivateVdpImageType(string designAtomsVariableType) =>
        designAtomsVariableType switch
        {
            "Image" => VdpImageType.Private,
            "ImagePlaceholder" => VdpImageType.PrivatePlaceholder,
            _ => VdpImageType.Private,
        };
}

internal class LineItemServiceMapperProfile : Profile
{
    public LineItemServiceMapperProfile()
    {
        CreateMap<LineItem, LineItem>();
    }
}
