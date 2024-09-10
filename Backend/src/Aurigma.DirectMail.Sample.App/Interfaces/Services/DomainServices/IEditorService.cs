using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with Customer's Canvas Design Editor.
/// </summary>
public interface IEditorService
{
    /// <summary>
    /// Returns a integration details data necessary to initialize the editor.
    /// </summary>
    /// <param name="reference">Product reference which will be personalized.</param>
    /// <returns>Integration details.</returns>
    /// <exception cref="CustomersCanvasProductNotFoundException">Product by product reference was not found.</exception>
    Task<IntegrationDetails> GetIntegrationDetailsAsync(string reference);

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
    Task<DesignValidationResult> ValidateDesignAsync(Guid lineItemId);

    /// <summary>
    /// Returns a available variables.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <returns>The collection of variables.</returns>
    Task<IEnumerable<EditorVariableInfo>> GetAvailableVariablesAsync(Guid lineItemId);

    /// <summary>
    /// Creates a design for personalization in the Design Editor.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <returns>The design for personalization.</returns>
    /// <remarks>If a resource of type EditorModel exists, generates a design from it. If not, then from public design.</remarks>
    Task<Design> CreateEditorDesignAsync(Guid lineItemId, string userId);
}
