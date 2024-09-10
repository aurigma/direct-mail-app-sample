using Aurigma.AssetProcessor;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class PrivateDesignProcessorAdapter(
    IPrivateDesignProcessorApiClient privateDesignProcessorApiClient
) : IPrivateDesignProcessorAdapter
{
    private readonly IPrivateDesignProcessorApiClient _privateDesignProcessorApiClient =
        privateDesignProcessorApiClient;

    public async Task<DesignDto> GenerateDesignFromPublicDesignAsync(
        string publicDesignId,
        string userId,
        string token
    )
    {
        try
        {
            _privateDesignProcessorApiClient.AuthorizationToken = token;

            var generateDesignModel = new CopyDesignFromPublicDesignModel()
            {
                PublicDesignId = publicDesignId,
            };

            var privateDesign =
                await _privateDesignProcessorApiClient.CopyDesignFromPublicDesignAsync(
                    ownerId: userId,
                    body: generateDesignModel
                );

            return privateDesign;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasDesignNotFoundException(
                publicDesignId,
                $"The design with identifier {publicDesignId} was not found",
                ex
            );
        }
    }

    public async Task<DesignDto> GenerateDesignFromPublicResourceAsync(
        string publicResourceId,
        string userId,
        string token
    )
    {
        try
        {
            _privateDesignProcessorApiClient.AuthorizationToken = token;

            var generateDesignModel = new CopyDesignFromPublicResourceModel()
            {
                PublicResourceId = publicResourceId,
            };

            var privateDesign =
                await _privateDesignProcessorApiClient.CopyDesignFromPublicResourceAsync(
                    ownerId: userId,
                    body: generateDesignModel
                );

            return privateDesign;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasResourceNotFoundException(
                publicResourceId,
                $"The resource with identifier {publicResourceId} was not found",
                ex
            );
        }
    }
}
