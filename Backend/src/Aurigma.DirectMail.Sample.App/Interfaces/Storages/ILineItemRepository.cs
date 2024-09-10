using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface ILineItemRepository
{
    Task<LineItem> GetLineItemByIdAsync(Guid id);
    Task<LineItem> GetLineItemByIdAsReadOnlyAsync(Guid id);

    Task<List<LineItem>> GetAllAsync();
    Task<List<LineItem>> GetAllAsReadOnlyAsync();
    Task<List<LineItem>> GetLineItemsByFilterAsync(LineItemFilter filter);

    Task<LineItem> CreateLineItemAsync(LineItem lineItem);
    Task<LineItem> UpdateLineItemAsync(LineItem lineItem);
    Task<LineItem> DeleteLineItemAsync(Guid id);
}
