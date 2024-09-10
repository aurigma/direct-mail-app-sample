using Aurigma.AssetStorage;
using Aurigma.DesignAtomsApi;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.IntegratedProduct;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.App.Models.Resource;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using Aurigma.StorefrontApi.Products;
using AutoMapper;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class IntegratedProductService(
    IProductService productService,
    IProductReferenceAdapter productReferenceAdapter,
    ITokenAdapter tokenAdapter,
    IProductAdapter productAdapter,
    IDesignAdapter designAdapter,
    IDesignAtomsServiceAdapter designAtomsServiceAdapter,
    IResourceAdapter resourceAdapter,
    IMapper mapper
) : IIntegratedProductService
{
    private readonly IProductService _productService = productService;
    private readonly IProductReferenceAdapter _productReferenceAdapter = productReferenceAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly IProductAdapter _productAdapter = productAdapter;
    private readonly IDesignAdapter _designAdapter = designAdapter;
    private readonly IDesignAtomsServiceAdapter _designAtomsServiceAdapter =
        designAtomsServiceAdapter;
    private readonly IResourceAdapter _resourceAdapter = resourceAdapter;
    private readonly IMapper _mapper = mapper;

    public async Task<List<IntegratedProduct>> GetIntegratedProductsAsync(
        IntegratedProductFilter filter
    )
    {
        var productFilter = new ProductFilter() { CategoryId = filter.CategoryId ?? null };

        var products = await _productService.GetProductsAsync(productFilter);
        if (!products.Any())
            return new List<IntegratedProduct>();

        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var productLinks = await GetCustomersCanvasProductLinks(token);
        if (!productLinks.Any())
            return new List<IntegratedProduct>();

        var linkedProducts = products
            .Where(p => productLinks.Any(pl => Equals(pl.StorefrontProductId, p.Id.ToString())))
            .Select(p => new IntegratedProduct
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                CategoryId = p.CategoryId,
                PreviewUrl =
                    productLinks
                        .First(pl => Equals(pl.StorefrontProductId, p.Id.ToString()))
                        .Image?.Url ?? null,
                CustomersCanvasProductId = productLinks
                    .First(pl => Equals(pl.StorefrontProductId, p.Id.ToString()))
                    .ProductId,
            });

        return linkedProducts.ToList();
    }

    public async Task<List<IntegratedProductOption>> GetIntegratedProductOptionsAsync(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var ccProduct = await GetProductByReferenceAsync(id.ToString(), token);

        var productOptions = await GetCustomersCanvasProductOptions(ccProduct.ProductId, token);
        if (!productOptions.Any())
            return new List<IntegratedProductOption>();

        var integratedProductOptions = productOptions.Select(option => new IntegratedProductOption
        {
            Id = option.Id,
            Title = option.Title,
            OptionType = _mapper.Map<IntegratedProductOptionType>(
                option.AppearanceData?.Type ?? AppearanceDataType.Radio
            ),
            Values =
                option.ProductOptionValues.Select(value => new IntegratedProductOptionValue
                {
                    Id = value.Id,
                    Title = value.Title,
                    SortIndex = value.SortIndex,
                }) ?? new List<IntegratedProductOptionValue>(),
        });

        return integratedProductOptions.ToList();
    }

    public async Task<List<IntegratedProductTemplate>> GetIntegratedProductTemplatesAsync(
        Guid id,
        IntegrationProductOptionRequestModel model
    )
    {
        var serializedOptions = ParseOptionsToKeyValueString(model.Options);

        // Request product model from Customer's Canvas.
        var product = await _productService.GetProductAsync(id);
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var ccProduct = await GetProductByReferenceAsync(id.ToString(), token);

        // Request product's variant designs.
        var productVariantDesigns = await GetCustomersCanvasProductVariantDesigns(
            ccProduct.ProductId,
            serializedOptions,
            model.TemplateTitle,
            token
        );

        // Creates list of integrated product template.
        var integratedProductTemplates = productVariantDesigns
            .Where(vd => vd.IsAvailable)
            .Select(vd => new IntegratedProductTemplate
            {
                TemplateId = vd.DesignId,
                TemplateName = vd.DesignName,
                PreviewUrl =
                    vd.ProductVariantResources.FirstOrDefault(r =>
                        r.Type is ProductVariantResourceType.Preview
                    )?.Url ?? null,
                ProductVariantId = vd.ProductVariantId,
            });

        return integratedProductTemplates.ToList();
    }

    public async Task<IntegratedProductTemplateDetails> GetIntegratedProductTemplateDetailsAsync(
        Guid id,
        IntegratedProductTemplateDetailsRequestModel model
    )
    {
        // Get Portal and Customer's Canvas product
        var product = await _productService.GetProductAsync(id);
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var ccProduct = await GetProductByReferenceAsync(id.ToString(), token);

        // Get Customer's Canvas design from Asset Storage API.
        var design = await _designAdapter.GetDesignAsync(model.TemplateId, token);

        // Get Customer's Canvas product variant designs for the current option set
        var productVariantDesigns = await GetCustomersCanvasProductVariantDesigns(
            productId: ccProduct.ProductId,
            options: string.Empty,
            search: string.Empty,
            token: token,
            productVariantId: model.ProductVariantId
        );

        var variantMockups = await _productAdapter.GetProductVarianMockupsAsync(
            productId: ccProduct.ProductId,
            options: null,
            templateTitle: null,
            token: token,
            productVariantId: model.ProductVariantId
        );

        if (
            !productVariantDesigns.Any()
            || !productVariantDesigns.Any(pv => pv.DesignId == model.TemplateId)
        )
        {
            throw new CustomersCanvasDesignNotConnectedException(
                model.TemplateId,
                id.ToString(),
                $"Product with reference {id} does not have designs connected to this product variant: {model.ProductVariantId}"
            );
        }

        // Search for a design variant connected to the desired template.
        var variantDesign = productVariantDesigns.Single(pv => pv.DesignId == model.TemplateId);

        // Request or generate high resolution previews.
        var highResolutionResources = await GetVariantDesignHighResPreviews(
            variantDesign,
            variantMockups.ToList(),
            model.TemplateId,
            token
        );

        // Creates a integrated product template details.
        var integratedProductTemplateDetails = new IntegratedProductTemplateDetails
        {
            TemplateId = design.Id,
            TemplateName = design.Name,
            ProductVariantId = model.ProductVariantId,
            CustomFields = design.CustomFields,
            PreviewUrls = highResolutionResources.Select(p => p.Url),
            Options = variantDesign.ProductVariantOptions?.Select(
                op => new IntegratedProductTemplateDetailsOption
                {
                    Title = op.ProductOptionTitle,
                    Value = op.ProductOptionValueTitle,
                }
            ),
        };

        return integratedProductTemplateDetails;
    }

    public async Task<IEnumerable<IntegratedProductResource>> GetIntegratedProductResourcesAsync(
        IntegratedProductResourcesRequestAppModel model
    )
    {
        // Get Portal and Customer's Canvas product
        var product = await _productService.GetProductAsync(model.ProductId);
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var ccProduct = await GetProductByReferenceAsync(model.ProductId.ToString(), token);

        // Get Customer's Canvas product variant designs for the current option set
        var productVariantDesigns = await GetCustomersCanvasProductVariantDesigns(
            productId: ccProduct.ProductId,
            options: string.Empty,
            search: string.Empty,
            token: token,
            productVariantId: model.ProductVariantId
        );

        if (
            !productVariantDesigns.Any()
            || !productVariantDesigns.Any(pv => pv.DesignId == model.TemplateId)
        )
        {
            throw new CustomersCanvasDesignNotConnectedException(
                model.TemplateId,
                model.ProductId.ToString(),
                $"Product with reference {model.ProductId} does not have designs connected to this product variant: {model.ProductVariantId}"
            );
        }

        // Search for a design variant connected to the desired template.
        var variantDesign = productVariantDesigns.Single(pv => pv.DesignId == model.TemplateId);

        var resources = variantDesign.ProductVariantResources.Select(
            r => new IntegratedProductResource()
            {
                Id = r.ResourceId,
                Name = r.Name,
                Url = r.Url,
                Type = _mapper.Map<IntegratedProductResourceType>(r.Type),
                Preview = new IntegratedProductResourcePreview
                {
                    Height = r.ResourcePreview?.Height,
                    MockupId = r.ResourcePreview?.MockupId,
                    Width = r.ResourcePreview?.Width,
                    SurfaceIndex = r.ResourcePreview?.SurfaceIndex,
                },
            }
        );

        return resources;
    }

    public async Task UpdateIntegratedProductResourcesAsync(Guid productId)
    {
        var product = await _productService.GetProductAsync(productId);
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var ccProduct = await GetProductByReferenceAsync(product.Id.ToString(), token);
        await _productAdapter.UpdateVariantResourcesAsync(ccProduct.ProductId, token);
    }

    private async Task<IEnumerable<ProductLinkDto>> GetCustomersCanvasProductLinks(string token)
    {
        var productLinks = await _productReferenceAdapter.GetProductLinks(token);

        return productLinks;
    }

    private async Task<IEnumerable<ProductOptionDto>> GetCustomersCanvasProductOptions(
        int productId,
        string token
    )
    {
        var productOptions = await _productAdapter.GetProductOptionsAsync(productId, token);

        return productOptions;
    }

    private async Task<ProductReferenceDto> GetProductByReferenceAsync(
        string productReference,
        string token
    )
    {
        var product = await _productReferenceAdapter.GetProductByReferenceAsync(
            token,
            productReference
        );

        return product;
    }

    private async Task<
        IEnumerable<ProductVariantDesignDto>
    > GetCustomersCanvasProductVariantDesigns(
        int productId,
        string options,
        string search,
        string token,
        int? productVariantId = null
    )
    {
        var productVariantDesigns = await _productAdapter.GetProductVariantDesignsAsync(
            productId,
            options,
            search,
            token,
            productVariantId
        );

        return productVariantDesigns;
    }

    private async Task<IEnumerable<ResourceDto>> GetVariantDesignHighResPreviews(
        ProductVariantDesignDto variantDesign,
        List<ProductVariantMockupDto> variantMockups,
        string templateId,
        string token
    )
    {
        var previewVariantMockups = variantMockups.Where(vm =>
            vm.MockupType is ProductVariantMockupType.Preview
        );

        var highResolutionResources = new List<ResourceDto>();
        foreach (var variantMockup in previewVariantMockups)
        {
            var surfaceIndex = variantMockup.SurfaceIndex.HasValue
                ? variantMockup.SurfaceIndex.Value
                : 0;
            var resourceRequestModel = new ResourceRequestAdapterModel
            {
                Namespace = "DirectMail-sample",
                SourceId = variantMockup.MockupId,
                Name =
                    variantDesign.ProductVariantId
                    + "_"
                    + templateId
                    + "_"
                    + variantMockup.MockupId
                    + "_"
                    + surfaceIndex
                    + "_800x800",
            };

            highResolutionResources.AddRange(
                await _resourceAdapter.GetResourcesAsync(resourceRequestModel, token)
            );
        }

        if (!highResolutionResources.Any())
        {
            foreach (var variantMockup in previewVariantMockups)
            {
                var surfaceIndex = variantMockup.SurfaceIndex.HasValue
                    ? variantMockup.SurfaceIndex.Value
                    : 0;

                var generateResourceAdapterModel = new DesignPreviewToResourceRequestAdapterModel
                {
                    Namespace = "DirectMail-sample",
                    SourceId = variantMockup.MockupId,
                    DesignId = templateId,
                    MockupId = variantMockup.MockupId,
                    SurfaceIndex = variantMockup.SurfaceIndex.HasValue
                        ? variantMockup.SurfaceIndex.Value
                        : 0,
                    ResourceName =
                        variantDesign.ProductVariantId
                        + "_"
                        + templateId
                        + "_"
                        + variantMockup.MockupId
                        + "_"
                        + surfaceIndex
                        + "_800x800",
                };

                var generatedResource =
                    await _designAtomsServiceAdapter.RenderDesignPreviewToResourceAsync(
                        generateResourceAdapterModel,
                        token
                    );
                highResolutionResources.Add(_mapper.Map<ResourceDto>(generatedResource));
            }
        }

        return highResolutionResources;
    }

    private string ParseOptionsToKeyValueString(
        IEnumerable<IntegrationProductOptionItemRequestModel> options
    )
    {
        try
        {
            if (!options.Any())
                return string.Empty;

            var optionsDictionary = options
                .Select(option => new KeyValuePair<int, string>(
                    option.OptionId,
                    string.Join(',', option.ValueIds)
                ))
                .ToDictionary();
            var serializedOptionDictionary = StringHelper.Serialize(optionsDictionary);
            return serializedOptionDictionary;
        }
        catch (Exception ex)
        {
            throw new IntegratedProductOptionsException(
                $"Failed parse options to {{ \"opt1_id\": \"opt1_val1_id, opt1_val2_id\", \"opt2_id\": \"opt2_val1_id\" }} format",
                ex
            );
        }
    }
}

internal class IntegratedProductServiceMapperProfile : Profile
{
    public IntegratedProductServiceMapperProfile()
    {
        CreateMap<AppearanceDataType, IntegratedProductOptionType>().ReverseMap();
        CreateMap<ResourceInfoDto, ResourceDto>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));

        CreateMap<ProductVariantResourceType, IntegratedProductResourceType>().ReverseMap();
    }
}
