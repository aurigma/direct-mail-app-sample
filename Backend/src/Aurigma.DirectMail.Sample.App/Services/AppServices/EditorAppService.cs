using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.Editor;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class EditorAppService(IEditorService editorService, ILogger<EditorAppService> logger)
    : IEditorAppService
{
    private readonly IEditorService _editorService = editorService;
    private readonly ILogger<EditorAppService> _logger = logger;

    public async Task<IntegrationDetails> GetIntegrationDetailsAsync(string productReference)
    {
        try
        {
            var integrationDetails = await _editorService.GetIntegrationDetailsAsync(
                productReference
            );
            LogSuccessRequestIntegrationDetails(integrationDetails);
            return integrationDetails;
        }
        catch (CustomersCanvasProductNotFoundException ex)
        {
            LogCustomersCanvasProductNotFound(ex, productReference);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestIntegrationDetails(ex, productReference);
            throw;
        }
    }

    public async Task<Token> GetEditorTokenAsync(string userId)
    {
        try
        {
            var token = await _editorService.GetEditorTokenAsync(userId);
            LogSuccessRequestToken(userId);
            return token;
        }
        catch (Exception ex)
        {
            LogErrorRequestToken(ex, userId);
            throw;
        }
    }

    public async Task<DesignValidationResult> ValidateDesignAsync(Guid lineItemId)
    {
        try
        {
            var validationResult = await _editorService.ValidateDesignAsync(lineItemId);
            LogSuccessValidateDesign(validationResult);
            return validationResult;
        }
        catch (LineItemNotFoundException ex)
        {
            LogValidateDesignLineItemNotFound(ex, lineItemId);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (LineItemDesignException ex)
        {
            LogInvalidAnValidateDesign(ex, lineItemId);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorAnValidateDesign(ex, lineItemId);
            throw;
        }
    }

    public async Task<IEnumerable<EditorVariableInfo>> GetAvailableVariablesAsync(Guid lineItemId)
    {
        try
        {
            var variables = await _editorService.GetAvailableVariablesAsync(lineItemId);
            LogSuccessRequestAvailableVariables(variables.Count());
            return variables;
        }
        catch (LineItemNotFoundException ex)
        {
            LogRequestAvailableVariablesLineItemNotFound(ex, lineItemId);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestAvailableVariables(ex, lineItemId);
            throw;
        }
    }

    public async Task<Design> CreateEditorDesignAsync(Guid lineItemId, string userId)
    {
        try
        {
            var generatedDesignId = await _editorService.CreateEditorDesignAsync(
                lineItemId,
                userId
            );
            LogSuccessDesignCreation(generatedDesignId);
            return generatedDesignId;
        }
        catch (EditorIdException ex)
        {
            LogInvalidADesignCreation(ex, lineItemId);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemDesignException ex)
        {
            LogInvalidADesignCreation(ex, lineItemId);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (LineItemProductVariantException ex)
        {
            LogInvalidADesignCreation(ex, lineItemId);
            throw new InvalidStateAppException("ProductVariantId", ex.Id, ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogDesignCreationLineItemNotFound(ex, lineItemId);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorDesignCreation(ex, lineItemId);
            throw;
        }
    }

    #region Logging

    private void LogSuccessRequestIntegrationDetails(IntegrationDetails editorInfo)
    {
        _logger.LogDebug(
            $"The integration details was returned. editorInfo={Serialize(editorInfo)}"
        );
    }

    private void LogCustomersCanvasProductNotFound(Exception ex, string reference)
    {
        _logger.LogWarning(
            ex,
            $"The Customer's canvas product with reference {reference} was not found"
        );
    }

    private void LogErrorRequestIntegrationDetails(Exception ex, string reference)
    {
        _logger.LogError(ex, $"Failed to request a integration details. reference={reference}");
    }

    private void LogSuccessRequestToken(string userId)
    {
        _logger.LogDebug($"The Design editor token was returned. userId={userId}");
    }

    private void LogErrorRequestToken(Exception ex, string userId)
    {
        _logger.LogError(ex, $"Failed to request a Design Editor token. userId={userId}");
    }

    private void LogSuccessValidateDesign(DesignValidationResult model)
    {
        _logger.LogDebug($"The design validated. validationResult={Serialize(model)}");
    }

    private void LogValidateDesignLineItemNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(
            ex,
            $"Failed to validate design. Line item with identifier {id} was not found"
        );
    }

    private void LogInvalidAnValidateDesign(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"Failed to validate design. lineItemId={id}");
    }

    private void LogErrorAnValidateDesign(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to validate design. lineItemId={id}");
    }

    private void LogSuccessRequestAvailableVariables(int variablesCount)
    {
        _logger.LogDebug($"The available variables was returned. variablesCount={variablesCount}");
    }

    private void LogRequestAvailableVariablesLineItemNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"Failed to request a available variables. lineItemId={id}");
    }

    private void LogErrorRequestAvailableVariables(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a available variables. lineItemId={id}");
    }

    private void LogSuccessDesignCreation(Design design)
    {
        _logger.LogDebug($"The private design was created. design={Serialize(design)}");
    }

    private void LogInvalidADesignCreation(Exception ex, Guid lineItemId)
    {
        _logger.LogWarning(ex, $"Failed to create a private design. lineItemId={lineItemId}");
    }

    private void LogDesignCreationLineItemNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"Failed to create a private design. lineItemId={id}");
    }

    private void LogErrorDesignCreation(Exception ex, Guid lineItemId)
    {
        _logger.LogError(ex, $"Failed to create a private design. lineItemId={lineItemId}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}
