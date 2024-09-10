namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations to operations with Customer's Canvas authorization token.
/// </summary>
public interface ITokenAdapter
{
    /// <summary>
    /// Returns a authorization token.
    /// </summary>
    /// <returns>Authorization token.</returns>
    Task<string> GetCustomersCanvasTokenAsync();
}
