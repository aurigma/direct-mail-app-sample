using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.StorefrontApi.Products;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas products.
/// </summary>
public interface IProductAdapter
{
    /// <summary>
    /// Returns a product options.
    /// </summary>
    /// <param name="productId">Customer's Canvas product id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Collection of product options.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">Product was not found.</exception>
    Task<IEnumerable<ProductOptionDto>> GetProductOptionsAsync(int productId, string token);

    /// <summary>
    /// Returns a product variant designs.
    /// </summary>
    /// <param name="productId">Customer's Canvas product id.</param>
    /// <param name="options">Serialized options model.</param>
    /// <param name="templateTitle">Search string for design(template) name partial match.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <param name="productVariantId">Customer's Canvas product variant id.</param>
    /// <returns>Collection of variant designs</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">Product was not found.</exception>
    Task<IEnumerable<ProductVariantDesignDto>> GetProductVariantDesignsAsync(
        int productId,
        string options,
        string templateTitle,
        string token,
        int? productVariantId = null
    );

    /// <summary>
    /// Returns a product variant mockups.
    /// </summary>
    /// <param name="productId">Customer's Canvas product id.</param>
    /// <param name="options">Serialized options model.</param>
    /// <param name="templateTitle">Search string for design(template) name partial match.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <param name="productVariantId">Customer's Canvas product variant id.</param>
    /// <returns>Collection of variant mockups.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">Product was not found.</exception>
    Task<IEnumerable<ProductVariantMockupDto>> GetProductVarianMockupsAsync(
        int productId,
        string options,
        string templateTitle,
        string token,
        int? productVariantId = null
    );

    /// <summary>
    /// Returns a product variant.
    /// </summary>
    /// <param name="productId">Customer's Canvas product id.</param>
    /// <param name="variantId">Customer's Canvas product variant id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The product variant.</returns>
    Task<ProductVariantDto> GetProductVariantAsync(int productId, int variantId, string token);

    /// <summary>
    /// Updates product variant resources.
    /// </summary>
    /// <param name="productId">Customer's Canvas product id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <exception cref="CustomersCanvasProductNotFoundException">Product was not found.</exception>
    Task UpdateVariantResourcesAsync(int productId, string token);
}
