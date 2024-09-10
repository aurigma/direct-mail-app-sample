using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface IRecipientListRepository
{
    Task<RecipientList> CreateRecipientListAsync(RecipientList list);
    Task<List<RecipientList>> GetAllAsReadOnlyAsync();
    Task<RecipientList> GetRecipientListAsReadOnlyAsync(Guid id);

    Task<List<RecipientList>> GetRecipientListsByFilterAsync(RecipientListFilter filter);
}
