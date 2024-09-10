using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with products that are integrated into Customer's Canvas.
/// </summary>
public interface IIntegratedProductAppService
{
    /// <summary>
    /// Return all integrated products by filter.
    /// </summary>
    /// <param name="model">Request model filter.</param>
    /// <returns>All integrated products satisfying the filter.</returns>
    Task<List<IntegratedProduct>> GetIntegratedProductsAsync(IntegratedProductRequestModel model);

    /// <summary>
    /// Returns a list of all integrated product options.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <returns>List of integrated product options.</returns>
    /// <exception cref="NotFoundAppException"> The product was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<List<IntegratedProductOption>> GetIntegratedProductOptionsAsync(Guid id);

    /// <summary>
    /// Returns a list of all integrated product templates.
    /// </summary>
    /// <param name="id">Product id.<param>
    /// <param name="model">Defines options filter</param>
    /// <returns>List of integrated product templates.</returns>
    /// <exception cref="NotFoundAppException"> The product was not found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<List<IntegratedProductTemplate>> GetIntegratedProductTemplatesAsync(
        Guid id,
        IntegrationProductOptionRequestModel model
    );

    /// <summary>
    /// Returns a integrated product template details.
    /// </summary>
    /// <param name="id">Product id.<param>
    /// <param name="model">Request model.</param>
    /// <returns> The integrated product template details.</returns>
    /// <exception cref="NotFoundAppException"> The product was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<IntegratedProductTemplateDetails> GetIntegratedProductTemplateDetailsAsync(
        Guid id,
        IntegratedProductTemplateDetailsRequestModel model
    );

    /// <summary>
    /// Updated integrated product resources.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <exception cref="NotFoundAppException"> The product was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task UpdateIntegratedProductResourcesAsync(Guid productId);
}
