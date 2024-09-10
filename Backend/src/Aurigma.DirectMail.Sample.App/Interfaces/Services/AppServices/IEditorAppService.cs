using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with Customer's Canvas Design Editor.
/// </summary>
public interface IEditorAppService
{
    /// <summary>
    /// Returns a integration details data necessary to initialize the editor.
    /// </summary>
    /// <param name="productReference">Product reference which will be personalized.</param>
    /// <returns>Integration details.</returns>
    /// <exception cref="NotFoundAppException">Product was not found.</exception>
    Task<IntegrationDetails> GetIntegrationDetailsAsync(string productReference);

    /// <summary>
    /// Returns a authorization token for Design Editor.
    /// </summary>
    /// <param name="userId">Storefront user id.</param>
    /// <returns>The authorization token for Design Editor.</returns>
    Task<Token> GetEditorTokenAsync(string userId);

    /// <summary>
    /// Validates a design for the presence of VDP variables.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <returns>Validation result.</returns>
    /// <exception cref="NotFoundAppException">Line item was not found.</exception>
    /// <exception cref="InvalidStateAppException">The value of the model property conflicts with the current state of the application service.</exception>
    Task<DesignValidationResult> ValidateDesignAsync(Guid lineItemId);

    /// <summary>
    /// Returns a available variables.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <returns>The collection of variables.</returns>
    /// <exception cref="NotFoundAppException">Line item was not found.</exception>
    Task<IEnumerable<EditorVariableInfo>> GetAvailableVariablesAsync(Guid lineItemId);

    /// <summary>
    /// Creates a design for personalization in the Design Editor.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <returns>The design for personalization.</returns>
    /// <exception cref="NotFoundAppException">Line item was not found.</exception>
    /// <exception cref="InvalidValueAppException">The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException">The value of the model property conflicts with the current state of the application service.</exception>
    Task<Design> CreateEditorDesignAsync(Guid lineItemId, string userId);
}
