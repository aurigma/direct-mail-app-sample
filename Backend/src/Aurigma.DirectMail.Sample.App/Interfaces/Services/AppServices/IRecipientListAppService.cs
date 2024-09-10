using Aurigma.DirectMail.Sample.App.Models.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with recipient lists.
/// </summary>
public interface IRecipientListAppService
{
    /// <summary>
    /// Returns all recipient lists.
    /// </summary>
    /// <returns> All recipient lists.</returns>
    Task<List<RecipientList>> GetRecipientListsAsync();

    /// <summary>
    /// Submit a recipient lists.
    /// </summary>
    /// <param name="model"></param>
    Task<IEnumerable<RecipientList>> SubmitRecipientListsAsync(RecipientListSubmitAppModel model);
}
