using Aurigma.DirectMail.Sample.App.Exceptions.Editor;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class EditorService(
    ILineItemService lineItemService,
    ICampaignService campaignService,
    IVdpService vdpService,
    IIntegratedProductService integratedProductService,
    ITenantInfoAdapter tenantInfoAdapter,
    ITokenAdapter tokenAdapter,
    IProductReferenceAdapter productReferenceAdapter,
    IStorefrontUserAdapter storefrontUserAdapter,
    IDesignAtomsServiceAdapter designAtomsServiceAdapter,
    IDesignEditorAdapter designEditorAdapter,
    IPrivateDesignProcessorAdapter privateDesignProcessorAdapter,
    IPrivateDesignAdapter privateDesignAdapter
) : IEditorService
{
    private readonly ILineItemService _lineItemService = lineItemService;
    private readonly ICampaignService _campaignService = campaignService;
    private readonly IVdpService _vdpService = vdpService;
    private readonly IIntegratedProductService _integratedProductService = integratedProductService;
    private readonly IDesignAtomsServiceAdapter _designAtomsServiceAdapter =
        designAtomsServiceAdapter;
    private readonly ITenantInfoAdapter _tenantInfoAdapter = tenantInfoAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly IProductReferenceAdapter _productReferenceAdapter = productReferenceAdapter;
    private readonly IStorefrontUserAdapter _storefrontUserAdapter = storefrontUserAdapter;
    private readonly IDesignEditorAdapter _designEditorAdapter = designEditorAdapter;
    private readonly IPrivateDesignProcessorAdapter _privateDesignProcessorAdapter =
        privateDesignProcessorAdapter;
    private readonly IPrivateDesignAdapter _privateDesignAdapter = privateDesignAdapter;

    public async Task<IntegrationDetails> GetIntegrationDetailsAsync(string reference)
    {
        // Request Customer's Canvas token
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();

        // Request product personalization workflow.
        var personalizationWorkflow =
            await _productReferenceAdapter.GetProductPersonalizationWorkflowAsync(reference, token);

        // Request installed applications info.
        var tenantApplicationInfo = await _tenantInfoAdapter.GetTenantApplicationInfoAsync(token);

        var integrationDetails = new IntegrationDetails
        {
            DesignEditorUrl = tenantApplicationInfo.DesignEditorUrl,
            ProductPersonalizationWorkflow = personalizationWorkflow,
        };

        return integrationDetails;
    }

    public async Task<Token> GetEditorTokenAsync(string userId)
    {
        // Request Customer's Canvas token
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();

        // Request installed applications info.
        var tenantApplicationInfo = await _tenantInfoAdapter.GetTenantApplicationInfoAsync(token);

        // Ensure create a Storefront user.
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            userId,
            token
        );

        // Request a Design Editor token for storefront user.
        var designEditorToken = await _designEditorAdapter.CreateTokenAsync(
            storefrontUser.UserId,
            tenantApplicationInfo.DesignEditorUrl,
            tenantApplicationInfo.DesignEditorApiKey
        );

        return designEditorToken;
    }

    public async Task<DesignValidationResult> ValidateDesignAsync(Guid lineItemId)
    {
        var lineItem = await _lineItemService.GetLineItemAsync(lineItemId);
        if (lineItem.DesignId == null || string.IsNullOrWhiteSpace(lineItem.DesignId))
            throw new LineItemDesignException(
                $"The line item with identifier {lineItem.Id} has no defined design"
            );

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            lineItem.CampaignId.ToString(),
            token
        );

        var personalizedDesignVariables = await _designAtomsServiceAdapter.GetVariablesAsync(
            lineItem.DesignId.ToString(),
            storefrontUser.UserId,
            token
        );

        var variables = personalizedDesignVariables
            .Items.Select(v => new Variable
            {
                Name = v.Name,
                Value = v.Value,
                Type = ToVariableType(v.Type),
            })
            .ToList();

        var validationResult = _vdpService.ValidateVariables(variables);

        var designValidationResult = new DesignValidationResult
        {
            MissingListVariableNames = validationResult.MissingRecipientVariableNames,
            MissingDesignVariableNames = validationResult.MissingSourceVariableNames,
        };

        return designValidationResult;
    }

    public async Task<IEnumerable<EditorVariableInfo>> GetAvailableVariablesAsync(Guid lineItemId)
    {
        var lineItem = await _lineItemService.GetLineItemAsync(lineItemId);
        var availableVariables = _vdpService.GetAvailableVariables();

        var editorVariables = availableVariables.Select(v => new EditorVariableInfo
        {
            Name = v.Name,
            Type = ToEditorVariableType(v.Type),
            BarcodeFormat = v.BarcodeFormat.HasValue
                ? ToEditorVariableBarcodeFormat(v.BarcodeFormat.Value)
                : null,
            BarcodeSubType = v.BarcodeSubType.HasValue
                ? ToEditorVariableBarcodeSubType(v.BarcodeSubType.Value)
                : null,
        });

        return editorVariables;
    }

    public async Task<Design> CreateEditorDesignAsync(Guid lineItemId, string userId)
    {
        ValidateUserId(userId);
        var lineItem = await _lineItemService.GetLineItemAsync(lineItemId);
        if (string.IsNullOrWhiteSpace(lineItem.TemplateId))
            throw new LineItemDesignException(
                lineItemId.ToString(),
                "The line item's design id is not specified."
            );

        if (!lineItem.ProductVariantId.HasValue)
            throw new LineItemProductVariantException(
                "The line item's product variant ID is not specified.",
                null
            );

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var storefrontUser = await _storefrontUserAdapter.EnsureCreateStorefrontUserAsync(
            userId,
            token
        );

        if (!string.IsNullOrWhiteSpace(lineItem.DesignId))
        {
            var existingDesignDto = await _privateDesignAdapter.GetPrivateDesignByIdAsync(
                lineItem.DesignId,
                storefrontUser.UserId,
                token
            );
            var existingDesign = BuildDesign(existingDesignDto);
            return existingDesign;
        }

        var requestResourcesAppModel = new IntegratedProductResourcesRequestAppModel
        {
            ProductId = lineItem.ProductId,
            ProductVariantId = lineItem.ProductVariantId.Value,
            TemplateId = lineItem.TemplateId,
            UserId = storefrontUser.UserId,
        };

        var resources = await _integratedProductService.GetIntegratedProductResourcesAsync(
            requestResourcesAppModel
        );
        AssetProcessor.DesignDto designDto;
        if (
            resources.Any()
            && resources.Any(r => r.Type is IntegratedProductResourceType.EditorModel)
        )
        {
            var editorModelResource = resources.FirstOrDefault(r =>
                r.Type is IntegratedProductResourceType.EditorModel
            );
            designDto = await _privateDesignProcessorAdapter.GenerateDesignFromPublicResourceAsync(
                editorModelResource.Id,
                storefrontUser.UserId,
                token
            );
            lineItem.DesignId = designDto.Id;
        }
        else
        {
            designDto = await _privateDesignProcessorAdapter.GenerateDesignFromPublicDesignAsync(
                lineItem.TemplateId,
                storefrontUser.UserId,
                token
            );
            lineItem.DesignId = designDto.Id;
        }

        var design = BuildDesign(designDto);

        await _lineItemService.UpdateLineItemAsync(lineItem);

        return design;
    }

    private VariableType ToVariableType(string type) =>
        type.ToLower() switch
        {
            "instring" => VariableType.InString,
            "text" => VariableType.Text,
            "imageplaceholder" => VariableType.PrivatePlaceholder,
            "image" => VariableType.PrivateImage,
            "barcode" => VariableType.Barcode,
            "barcodeplaceholder" => VariableType.BarcodePlaceholder,
            _ => throw new NotImplementedException(
                $"Unsupported variable type. variableType={type}"
            ),
        };

    private EditorVariableInfoType ToEditorVariableType(VariableType type) =>
        type switch
        {
            VariableType.PublicImage => EditorVariableInfoType.CustomImage,
            VariableType.PrivateImage => EditorVariableInfoType.CustomImage,
            VariableType.ExternalImage => EditorVariableInfoType.CustomImage,
            VariableType.Text => EditorVariableInfoType.Text,
            VariableType.InString => EditorVariableInfoType.Text,
            VariableType.Barcode => EditorVariableInfoType.CustomBarcode,
            VariableType.BarcodePlaceholder => EditorVariableInfoType.CustomBarcode,
            _ => throw new NotImplementedException(
                $"Unsupported variable type. variableType={type}"
            ),
        };

    private EditorVariableInfoBarcodeFormat? ToEditorVariableBarcodeFormat(
        VariableBarcodeFormat? type
    ) =>
        type switch
        {
            VariableBarcodeFormat.EAN_13 => EditorVariableInfoBarcodeFormat.EAN_13,
            VariableBarcodeFormat.EAN_8 => EditorVariableInfoBarcodeFormat.EAN_8,
            VariableBarcodeFormat.QR_CODE => EditorVariableInfoBarcodeFormat.QR_CODE,
            _ => throw new NotImplementedException(
                $"Unsupported barcode format. VariableBarcodeFormat={type}"
            ),
        };

    private EditorVariableInfoBarcodeSubType? ToEditorVariableBarcodeSubType(
        VariableBarcodeSubType? type
    ) =>
        type switch
        {
            VariableBarcodeSubType.Url => EditorVariableInfoBarcodeSubType.Url,
            VariableBarcodeSubType.Phone => EditorVariableInfoBarcodeSubType.Phone,
            VariableBarcodeSubType.None => EditorVariableInfoBarcodeSubType.None,
            _ => throw new NotImplementedException(
                $"Unsupported barcode sub type. VariableBarcodeSubType={type}"
            ),
        };

    private Design BuildDesign(AssetProcessor.DesignDto designDto) =>
        new Design
        {
            Id = designDto.Id,
            CustomFields = designDto.CustomFields,
            Name = designDto.Name,
            OwnerId = designDto.OwnerId,
            Private = designDto.Private,
            Size = designDto.Size,
        };

    private Design BuildDesign(AssetStorage.DesignDto designDto) =>
        new Design
        {
            Id = designDto.Id,
            CustomFields = designDto.CustomFields,
            Name = designDto.Name,
            OwnerId = designDto.OwnerId,
            Private = designDto.Private,
            Size = designDto.Size,
        };

    private void ValidateUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new EditorIdException("The user id is not specified", userId);
        }
    }
}
