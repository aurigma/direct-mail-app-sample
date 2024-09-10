using Aurigma.DesignAtomsApi;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.App.Models.VDP;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class DesignAtomsServiceAdapter(IDesignAtomsServiceApiClient designAtomsServiceApiClient)
    : IDesignAtomsServiceAdapter
{
    private readonly IDesignAtomsServiceApiClient _designAtomsServiceApiClient =
        designAtomsServiceApiClient;

    public async Task<FileResponse> RenderDesignProofAsync(
        ProofRequestAdapterModel model,
        string token
    )
    {
        try
        {
            _designAtomsServiceApiClient.AuthorizationToken = token;

            var renderProofModel = new RenderDesignProofModel
            {
                DesignId = model.DesignId,
                OwnerId = model.UserId,
                RenderingConfig = model.Config,
                VariableData = model.Variables.ToList(),
            };

            var designProof = await _designAtomsServiceApiClient.RenderDesignProofAsync(
                attachment: false,
                body: renderProofModel
            );
            return designProof;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasPrivateDesignNotFoundException(
                model.DesignId,
                $"The private design with identifier {model.DesignId} was not found in user's with identifier {model.UserId} storage"
            );
        }
    }

    public async Task<FileResponse> RenderDesignPreviewAsync(
        PreviewRequestAdapterModel model,
        string token
    )
    {
        try
        {
            _designAtomsServiceApiClient.AuthorizationToken = token;

            var renderPreviewModel = new RenderDesignPreviewModel
            {
                DesignId = model.DesignId,
                MockupId = model.MockupId,
                OwnerId = model.UserId,
                RenderingConfig = model.Config,
                VariableData = model.Variables?.ToList(),
            };

            var designPreview = await _designAtomsServiceApiClient.RenderDesignPreviewAsync(
                attachment: false,
                body: renderPreviewModel
            );
            return designPreview;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasPrivateDesignNotFoundException(
                model.DesignId,
                $"The private design with identifier {model.DesignId} was not found in user's with identifier {model.UserId} storage"
            );
        }
    }

    public async Task<VariablesModel> GetVariablesAsync(
        string designId,
        string userId,
        string token
    )
    {
        try
        {
            _designAtomsServiceApiClient.AuthorizationToken = token;

            var variables = await _designAtomsServiceApiClient.GetVariablesAsync(
                id: designId,
                privateStorage: true,
                privateStorageOwner: userId
            );
            return variables;
        }
        catch (ApiClientException ex) when (ex.StatusCode is 404)
        {
            throw new CustomersCanvasPrivateDesignNotFoundException(
                designId,
                $"The private design with identifier {designId} was not found in user's with identifier {userId} storage"
            );
        }
    }

    public async Task SendVdpDataAsync(VdpSendDataAdapterModel model, string token)
    {
        _designAtomsServiceApiClient.AuthorizationToken = token;

        var saveVdpDataModel = new VdpDataModel()
        {
            VdpItemsData = null,
            VdpDataSet = model.DataSet,
        };
        await _designAtomsServiceApiClient.SaveVdpDataAsync(
            id: model.DesignId,
            privateStorage: true,
            privateStorageOwner: model.UserId,
            body: saveVdpDataModel
        );
    }

    public async Task<ResourceInfoDto> RenderDesignPreviewToResourceAsync(
        DesignPreviewToResourceRequestAdapterModel model,
        string token
    )
    {
        _designAtomsServiceApiClient.AuthorizationToken = token;

        var requestModel = new RenderDesignPreviewToResourceModel
        {
            DesignId = model.DesignId,
            MockupId = model.MockupId,
            OwnerId = model.UserId,
            VariableData = model.Variables,
            PreviewResourceParams = new ResourceParams
            {
                AnonymousAccess = model.UserId is null ? true : false,
                OwnerId = model.UserId,
                Type = "Preview",
                Name = model.ResourceName,
                Namespace = model.Namespace,
                SourceId = model.SourceId,
            },
            RenderingConfig = new ProductPreviewRenderingConfig
            {
                FileFormat = ProductPreviewFormat.Jpeg,
                Height = 800,
                Width = 800,
                SurfaceIndex = model.SurfaceIndex,
            },
        };

        var resource = await _designAtomsServiceApiClient.RenderDesignPreviewToResourceAsync(
            body: requestModel
        );

        return resource;
    }
}
