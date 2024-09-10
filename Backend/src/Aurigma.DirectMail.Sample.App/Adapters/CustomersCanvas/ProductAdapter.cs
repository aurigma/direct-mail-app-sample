using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.StorefrontApi.Products;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class ProductAdapter(IProductsApiClient productsApiClient) : IProductAdapter
{
    private readonly IProductsApiClient _productsApiClient = productsApiClient;

    public async Task<IEnumerable<ProductVariantDesignDto>> GetProductVariantDesignsAsync(
        int productId,
        string options,
        string templateTitle,
        string token,
        int? productVariantId = null
    )
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;

            var variantDesignsPage = await _productsApiClient.GetProductVariantDesignsAsync(
                productId,
                options: options,
                search: templateTitle,
                productVariantId: productVariantId
            );

            return variantDesignsPage.Items;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                productId.ToString(),
                $"The product with identifier {productId} was not found ",
                ex
            );
        }
    }

    public async Task<IEnumerable<ProductVariantMockupDto>> GetProductVarianMockupsAsync(
        int productId,
        string options,
        string templateTitle,
        string token,
        int? productVariantId = null
    )
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;

            var variantMockupsPage = await _productsApiClient.GetProductVariantMockupsAsync(
                productId,
                options: options,
                search: templateTitle,
                productVariantId: productVariantId
            );

            return variantMockupsPage.Items;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                productId.ToString(),
                $"The product with identifier {productId} was not found ",
                ex
            );
        }
    }

    public async Task<IEnumerable<ProductOptionDto>> GetProductOptionsAsync(
        int productId,
        string token
    )
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;

            var productOptionsPage = await _productsApiClient.GetProductOptionsAsync(productId);
            return productOptionsPage.Items;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                productId.ToString(),
                $"The product with identifier {productId} was not found",
                ex
            );
        }
    }

    public async Task<ProductVariantDto> GetProductVariantAsync(
        int productId,
        int variantId,
        string token
    )
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;
            var productVariant = await _productsApiClient.GetProductVariantAsync(
                productId,
                variantId
            );
            return productVariant;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductVariantNotFoundException(
                variantId.ToString(),
                $"The product variant with identifier {variantId} was not found",
                ex
            );
        }
    }

    public async Task UpdateVariantResourcesAsync(int productId, string token)
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;

            await _productsApiClient.UpdateProductVariantResourcesAsync(productId);
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                productId.ToString(),
                $"The product with identifier {productId} was not found ",
                ex
            );
        }
    }
}
