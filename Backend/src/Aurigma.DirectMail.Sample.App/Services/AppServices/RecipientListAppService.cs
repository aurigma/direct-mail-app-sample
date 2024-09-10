using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class RecipientListAppService(
    ILogger<RecipientListAppService> logger,
    IRecipientListService recipientListService
) : IRecipientListAppService
{
    private readonly IRecipientListService _recipientListService = recipientListService;
    private readonly ILogger<RecipientListAppService> _logger = logger;

    public async Task<List<RecipientList>> GetRecipientListsAsync()
    {
        try
        {
            var lists = await _recipientListService.GetRecipientListsAsync();
            LogSuccessGetting(lists.Count);
            return lists;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex);
            throw;
        }
    }

    public async Task<IEnumerable<RecipientList>> SubmitRecipientListsAsync(
        RecipientListSubmitAppModel model
    )
    {
        try
        {
            var lists = await _recipientListService.SubmitRecipientListsAsync(model);
            LogSuccessSubmit(model);
            return lists;
        }
        catch (Exception ex)
        {
            LogErrorSubmit(model, ex);
            throw;
        }
    }

    #region Logging

    private void LogSuccessGetting(int listsCount)
    {
        _logger.LogDebug($"Recipient lists was returned. Lists count ={listsCount}");
    }

    private void LogErrorGetting(Exception ex)
    {
        _logger.LogError(ex, $"Failed to request recipient lists");
    }

    private void LogSuccessSubmit(RecipientListSubmitAppModel model)
    {
        _logger.LogDebug($"Recipient lists was submitted. requestModel={Serialize(model)}");
    }

    private void LogErrorSubmit(RecipientListSubmitAppModel model, Exception ex)
    {
        _logger.LogError(
            ex,
            $"Failed to submit a recipient lists. requestModel={Serialize(model)}"
        );
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}
