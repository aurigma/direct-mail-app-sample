using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with line items.
/// </summary>
public interface ILineItemService
{
    /// <summary>
    /// Creates a new line item.
    /// </summary>
    /// <param name="lineItem">Model to create a line item.</param>
    /// <returns>The created line item.</returns>
    /// <exception cref="LineItemIdException"> The IDs has an invalid value.</exception>
    /// <exception cref="LineItemCampaignException"> The line item's company has an invalid value.</exception>
    /// <exception cref="LineItemProductException"> The line item's product has an invalid value.</exception>
    Task<LineItem> CreateLineItemAsync(LineItem lineItem);

    /// <summary>
    /// Returns a line item by id.
    /// </summary>
    /// <param name="id">ID of the line item to search for.</param>
    /// <returns>The found line item.</returns>
    /// <exception cref="LineItemNotFoundException"> The line item was not found.</exception>
    Task<LineItem> GetLineItemAsync(Guid id);

    /// <summary>
    /// Return all line items by filter.
    /// </summary>
    /// <param name="filter">Request filter model.</param>
    /// <returns>All line items satisfying the filter.</returns>
    Task<List<LineItem>> GetLineItemsAsync(LineItemFilter filter);

    /// <summary>
    /// Updates a line item.
    /// </summary>
    /// <param name="lineItem">Model to update a line item.</param>
    /// <returns>The updated line item.</returns>
    /// <exception cref="LineItemNotFoundException"> The line item was not found.</exception>
    /// <exception cref="LineItemIdException"> The IDs has an invalid value.</exception>
    /// <exception cref="LineItemCampaignException"> The line item's company has an invalid value.</exception>
    /// <exception cref="LineItemProductException"> The line item's product has an invalid value.</exception>
    Task<LineItem> UpdateLineItemAsync(LineItem lineItem);

    /// <summary>
    /// Process finish line item personalization.
    /// </summary>
    /// <param name="id">Line item id.</param>
    /// <exception cref="LineItemNotFoundException"> The line item was not found.</exception>
    /// <exception cref="LineItemCampaignException"> The line item's company has an invalid value.</exception>
    /// <exception cref="LineItemDesignException"> The line item's design has an invalid value.</exception>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">Private design was not found.</exception>
    Task FinishLineItemPersonalizationAsync(Guid id);
}
