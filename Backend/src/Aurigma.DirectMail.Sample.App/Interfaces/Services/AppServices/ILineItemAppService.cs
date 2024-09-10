using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with line items.
/// </summary>
public interface ILineItemAppService
{
    /// <summary>
    /// Creates a new line item.
    /// </summary>
    /// <param name="model">Model to create a line item.</param>
    /// <returns>The created line item.</returns>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    Task<LineItem> CreateLineItemAsync(LineItemCreationModel model);

    /// <summary>
    /// Returns a product by id.
    /// </summary>
    /// <param name="id">ID of the line item to search for.</param>
    /// <returns>The found line item.</returns>
    /// <exception cref="NotFoundAppException"> The line item was not found.</exception>
    Task<LineItem> GetLineItemByIdAsync(Guid id);

    /// <summary>
    /// Return all line items by filter.
    /// </summary>
    /// <param name="model">Request filter model.</param>
    /// <returns>All line items satisfying the filter.</returns>
    Task<IEnumerable<LineItem>> GetLineItemsAsync(LineItemRequestModel model);

    /// <summary>
    /// Updates a line item.
    /// </summary>
    /// <param name="model">Model to update a line item.</param>
    /// <returns>The updated line item.</returns>
    /// <exception cref="NotFoundAppException"> The line item was not found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    Task<LineItem> UpdateLineItemAsync(LineItemUpdateModel model);

    /// <summary>
    /// Process finish line item personalization.
    /// </summary>
    /// <param name="id">Line item id.</param>
    /// <exception cref="NotFoundAppException"> The line item was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The property of model has an invalid value.</exception>
    Task FinishLineItemPersonalizationAsync(Guid id);
}
