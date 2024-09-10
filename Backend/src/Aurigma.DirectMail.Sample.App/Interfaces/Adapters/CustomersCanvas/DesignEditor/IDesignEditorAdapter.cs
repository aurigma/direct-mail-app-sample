using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas.DesignEditor;

/// <summary>
/// Adapter for performing operations with Customer's Canvas Design Editor.
/// </summary>
public interface IDesignEditorAdapter
{
    /// <summary>
    /// Returns a Design Editor authorization token.
    /// </summary>
    /// <param name="userId">Storefront user id.</param>
    /// <param name="apiUrl">Design Editor url.</param>
    /// <param name="apiKey">Design Editor API key.</param>
    /// <param name="updateOnCall">If true refreshes the token when requesting again.</param>
    /// <param name="seconds">Token lifetime.</param>
    /// <returns>The Design Editor authorization token</returns>
    /// <exception cref="DesignEditorAdapterException"> Request exception.</exception>
    Task<Token> CreateTokenAsync(
        string userId,
        string apiUrl,
        string apiKey,
        bool updateOnCall = true,
        int seconds = 7200
    );
}
