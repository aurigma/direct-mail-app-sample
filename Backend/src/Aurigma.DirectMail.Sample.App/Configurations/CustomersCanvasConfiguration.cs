namespace Aurigma.DirectMail.Sample.App.Configurations;

public class CustomersCanvasConfiguration
{
    /// <summary>
    /// Customer's Canvas backend URL.
    /// </summary>
    public string ApiGatewayUrl { get; set; }

    /// <summary>
    /// Tenant identifier.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Storefront identifier.
    /// </summary>
    public int StorefrontId { get; set; }

    /// <summary>
    /// External application client identifier. Need to get security tokens for storefront users.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// External application client secret key.
    /// </summary>
    public string ClientSecret { get; set; }
}
