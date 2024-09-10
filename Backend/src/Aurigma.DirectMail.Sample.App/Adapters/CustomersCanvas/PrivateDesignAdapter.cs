using Aurigma.AssetStorage;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class PrivateDesignAdapter(IPrivateDesignsApiClient privateDesignsApiClient)
    : IPrivateDesignAdapter
{
    private readonly IPrivateDesignsApiClient _privateDesignsApiClient = privateDesignsApiClient;

    public async Task<DesignDto> GetPrivateDesignByIdAsync(
        string designId,
        string userId,
        string token
    )
    {
        try
        {
            _privateDesignsApiClient.AuthorizationToken = token;
            var privateDesign = await _privateDesignsApiClient.GetAsync(designId, ownerId: userId);
            if (privateDesign.Id == null)
                throw new CustomersCanvasPrivateDesignNotFoundException(
                    designId,
                    $"The private design with identifier {designId} was not found"
                );

            return privateDesign;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasPrivateDesignNotFoundException(
                designId,
                $"The private design with identifier {designId} was not found",
                ex
            );
        }
    }

    public async Task<DesignDto> CopyPrivateDesignAsync(
        string sourceDesignId,
        string token,
        string userId
    )
    {
        try
        {
            _privateDesignsApiClient.AuthorizationToken = token;

            var copiedDesign = await _privateDesignsApiClient.CopyAsync(
                id: sourceDesignId,
                strategy: ConflictResolvingStrategy.Rename,
                ownerId: userId
            );

            return copiedDesign;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasPrivateDesignNotFoundException(
                sourceDesignId,
                $"The private design with identifier {sourceDesignId} was not found",
                ex
            );
        }
    }
}
