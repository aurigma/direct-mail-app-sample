using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.IntegratedProduct;
using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with products that are integrated into Customer's Canvas.
/// </summary>
public interface IIntegratedProductService
{
    /// <summary>
    /// Return all integrated products by filter.
    /// </summary>
    /// <param name="filter">Request filter model.</param>
    /// <returns>All integrated products satisfying the filter.</returns>
    Task<List<IntegratedProduct>> GetIntegratedProductsAsync(IntegratedProductFilter filter);

    /// <summary>
    /// Returns a list of all integrated product options.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <returns>List of integrated product options.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException"> The Customer's canvas product was not found at the reference.</exception>
    /// <exception cref="ProductNotFoundException"> The product was not found.</exception>
    Task<List<IntegratedProductOption>> GetIntegratedProductOptionsAsync(Guid id);

    /// <summary>
    /// Returns a list of all integrated product templates.
    /// </summary>
    /// <param name="id">Product id.<param>
    /// <param name="options">Defines options filter</param>
    /// <returns>List of integrated product templates.</returns>
    /// <exception cref="IntegratedProductOptionsException"> The property of options filter has an invalid value.</exception>
    /// <exception cref="ProductNotFoundException"> The product was not found.</exception>
    /// <exception cref="CustomersCanvasProductNotFoundException"> The Customer's canvas product was not found at the reference.</exception>
    Task<List<IntegratedProductTemplate>> GetIntegratedProductTemplatesAsync(
        Guid id,
        IntegrationProductOptionRequestModel model
    );

    /// <summary>
    /// Returns a integrated product template details.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <param name="templateId">Template id.</param>
    /// <returns> The integrated product template details.</returns>
    /// <exception cref="ProductNotFoundException"> The product was not found.</exception>
    /// <exception cref="CustomersCanvasDesignNotFoundException">The Customer's canvas design was not found at the ID.</exception>
    /// <exception cref="CustomersCanvasDesignNotConnectedException">The template was not connected to the product.</exception>
    Task<IntegratedProductTemplateDetails> GetIntegratedProductTemplateDetailsAsync(
        Guid id,
        IntegratedProductTemplateDetailsRequestModel model
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IEnumerable<IntegratedProductResource>> GetIntegratedProductResourcesAsync(
        IntegratedProductResourcesRequestAppModel model
    );

    /// <summary>
    /// Updates a product variant resources.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <exception cref="ProductNotFoundException"> The product was not found.</exception>
    /// <exception cref="CustomersCanvasProductNotFoundException"> The Customer's canvas product was not found at the reference.</exception>
    Task UpdateIntegratedProductResourcesAsync(Guid productId);
}
