using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.StorefrontApi.Products;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas storefront product references.
/// </summary>
public interface IProductReferenceAdapter
{
    /// <summary>
    /// Returns a list of product links associated with product references with current Storefront.
    /// </summary>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>The list of product links</returns>
    Task<IEnumerable<ProductLinkDto>> GetProductLinks(string token);

    /// <summary>
    /// Returns a product reference.
    /// </summary>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <param name="referenceId">Product reference.</param>
    /// <returns>Product reference DTO.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">The product reference was not found.</exception>
    /// <remarks>Product reference is the id of the Portal product.</remarks>
    Task<ProductReferenceDto> GetProductByReferenceAsync(string token, string referenceId);

    /// <summary>
    /// Returns a product personalization workflow.
    /// </summary>
    /// <param name="reference">Product reference.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Serialized product personalization workflow.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">The product reference was not found.</exception>
    /// <remarks>Product personalization workflow is used to configuring the Design Editor.</remarks>
    Task<string> GetProductPersonalizationWorkflowAsync(string reference, string token);
}
