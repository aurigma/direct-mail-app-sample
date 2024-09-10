using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Models.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with recipient lists.
/// </summary>
public interface IRecipientListService
{
    /// <summary>
    /// Returns all recipient lists.
    /// </summary>
    /// <param name="filter">Recipient list filter.</param>
    /// <returns> All recipient lists.</returns>
    Task<List<RecipientList>> GetRecipientListsAsync(RecipientListFilter filter = null);

    /// <summary>
    /// Returns a recipient list by id.
    /// </summary>
    /// <param name="id">Recipient list identifier.</param>
    /// <returns>The recipient list.</returns>
    Task<RecipientList> GetRecipientListAsync(Guid id);

    /// <summary>
    /// Submit a recipient lists.
    /// </summary>
    /// <param name="model"></param>
    Task<List<RecipientList>> SubmitRecipientListsAsync(RecipientListSubmitAppModel model);
}
