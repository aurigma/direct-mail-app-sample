using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.StorefrontApi.Products;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class ProductReferenceAdapter(
    IProductReferencesApiClient productsApiClient,
    IOptionsSnapshot<CustomersCanvasConfiguration> snapshot
) : IProductReferenceAdapter
{
    private readonly IProductReferencesApiClient _productsApiClient = productsApiClient;

    private readonly CustomersCanvasConfiguration _configuration = snapshot.Value;

    public async Task<IEnumerable<ProductLinkDto>> GetProductLinks(string token)
    {
        _productsApiClient.AuthorizationToken = token;

        var productLinkPage = await _productsApiClient.GetAllProductLinksAsync(
            _configuration.StorefrontId
        );

        return productLinkPage.Items;
    }

    public async Task<ProductReferenceDto> GetProductByReferenceAsync(
        string token,
        string referenceId
    )
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;

            var product = await _productsApiClient.GetAsync(
                referenceId,
                _configuration.StorefrontId
            );
            return product;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                referenceId,
                $"The product with reference {referenceId} was not found ",
                ex
            );
        }
    }

    public async Task<string> GetProductPersonalizationWorkflowAsync(string reference, string token)
    {
        try
        {
            _productsApiClient.AuthorizationToken = token;
            var personalizationWorkflow =
                await _productsApiClient.GetProductPersonalizationWorkflowAsync(
                    reference,
                    _configuration.StorefrontId
                );
            return personalizationWorkflow;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasProductNotFoundException(
                reference,
                $"The product with reference {reference} was not found ",
                ex
            );
        }
    }
}
